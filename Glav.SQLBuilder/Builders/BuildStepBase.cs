using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.SQLManagement;
using System.IO;

namespace Glav.SQLBuilder.Builders
{
    public abstract class BuildStepBase: IBuildStep
    {

        public BuildStepBase(IConfig config, ILogger logger, ISqlServer databaseServer)
        {
            Configuration = config;
            Logger = logger;
            SQLServer = databaseServer;
        }

        protected ILogger Logger { get; private set; }
        protected IConfig Configuration { get; private set; }
        protected ISqlServer SQLServer { get; private set; }

        public abstract BuildStepResult ExecuteBuildStep();
        public abstract string Name {get; }

		protected string SubstituteScriptContentBasedOnMode(string rawScriptContents)
		{
			switch (Configuration.BuildMode)
			{
				case SqlBuildMode.Staging:
					Logger.LogMessage("In Staging mode, performing script substitution for staging config values.");
					return PerformSubstitution(rawScriptContents, Configuration.ScriptSubstitutionsStaging);
					break;
				case SqlBuildMode.Release:
					Logger.LogMessage("In Release mode, performing script substitution for release config values.");
					return PerformSubstitution(rawScriptContents, Configuration.ScriptSubstitutionsRelease);
					break;
				case SqlBuildMode.Normal:
					Logger.LogMessage("In Normal mode, performing no script substitution.");
					return rawScriptContents;
				default:
					Logger.LogMessage("In Normal mode, performing no script substitution.");
					return rawScriptContents;
			}
		}

		private string PerformSubstitution(string originalScript, Dictionary<string, string> keyValuesToSubstitute)
		{
			if (string.IsNullOrWhiteSpace(originalScript) || keyValuesToSubstitute == null || keyValuesToSubstitute.Count == 0)
				return originalScript;

			StringBuilder substScript = new StringBuilder(originalScript);
			foreach (var kvp in keyValuesToSubstitute)
			{
				substScript.Replace(kvp.Key, kvp.Value);
			}

			return substScript.ToString();
		}

		protected void ExecuteOrOutputScript(string originalFilename, string scriptPrefix, string scriptContents)
		{
			if (Configuration.PackageOnly)
			{
				Logger.LogMessage("Packaging {0} script '{1}'", scriptPrefix, originalFilename);
				string scriptNewName = RenameScriptBasedOnBuildMode(originalFilename);
				Logger.LogMessage("{0} script renamed from '{1}' to '{2}'", scriptPrefix, originalFilename,scriptNewName);
				File.WriteAllText(scriptNewName,scriptContents);
				Logger.LogMessage("{0} script '{1}' packaged. Written to '{2}'", scriptPrefix, originalFilename, scriptNewName);
			}
			else
			{
				Logger.LogMessage("About to execute {0} script '{1}'", scriptPrefix, originalFilename);
				var count = SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(scriptContents);
				Logger.LogMessage("{0} script '{1}' executed. {2} items affected.", scriptPrefix, originalFilename, count);
			}
		}

		private string RenameScriptBasedOnBuildMode(string originalFilename)
		{
			if (string.IsNullOrWhiteSpace(originalFilename) || originalFilename.Length < 4 || Configuration.BuildMode == SqlBuildMode.Normal)
				return originalFilename;

			string nameNoExtension = originalFilename.Substring(0, originalFilename.Length - 4);
			string suffix = Configuration.BuildMode == SqlBuildMode.Staging ? "_Staging" : "_Release";
			return string.Format("{0}{1}.sql", nameNoExtension, suffix);
		}


    }
}
