using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Glav.SQLBuilder.Helpers;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;

namespace Glav.SQLBuilder.Builders
{
    public class ScriptExecutionStepBase : BuildStepBase
    {
        public ScriptExecutionStepBase(ILogger logger, IConfig config, ISqlServer databaseServer) : base(config, logger, databaseServer) { }

        public ScriptExecutionPhase ExecutionPhase { get; set; }

        public override BuildStepResult ExecuteBuildStep()
        {
            var result = new BuildStepResult(true, null);
            try
            {
                SQLServer.EnsureConnection();

                string preScriptFile = ExecutionPhase == ScriptExecutionPhase.PreBuild ? Configuration.PreScriptToRun : Configuration.PostScriptToRun;

                if (!string.IsNullOrWhiteSpace(preScriptFile))
                {
                    StringBuilder fullPath = new StringBuilder();
                    fullPath.Append(Configuration.ScriptDirectory);
                    if (Configuration.ScriptDirectory[Configuration.ScriptDirectory.Length - 1] != '\\')
                        fullPath.Append('\\');
                    fullPath.Append(preScriptFile);
                    var preBuildFile = fullPath.ToString();

                    if (File.Exists(preBuildFile))
                    {
                        var contentsOfFile = File.ReadAllText(preBuildFile);
                        SQLServer.DatabaseServer.ConnectionContext.BeginTransaction();
                        SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(string.Format("USE [{0}]", Configuration.DatabaseName));

                        Logger.LogMessage("About to execute {0} Script '{1}'", ExecutionPhase.ToString(), preBuildFile);
                        var count = SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(contentsOfFile);
                        Logger.LogMessage("{0} script '{1}' executed. {2} items affected.", ExecutionPhase.ToString(), preBuildFile, count);
                        SQLServer.DatabaseServer.ConnectionContext.CommitTransaction();

                    }
                }
                else
                {
                    Logger.LogMessage("{0} script not executed as no script was defined.", ExecutionPhase.ToString());
                }
            }
            catch (Exception ex)
            {
                SQLServer.DatabaseServer.ConnectionContext.RollBackTransaction();
                result.Successful = false;
                result.CanContinue = false;
                result.ResultDetail = ex.GetFullExceptionMessages();
            }

            return result;
        }

        public override string Name
        {
            get { return string.Format("{0} Script Execution",ExecutionPhase); }
        }
    }

    public enum ScriptExecutionPhase
    {
        PreBuild,
        PostBuild
    }
}
