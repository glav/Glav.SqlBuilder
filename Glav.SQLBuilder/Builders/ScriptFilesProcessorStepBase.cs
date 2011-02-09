using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Helpers;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;

namespace Glav.SQLBuilder.Builders
{
    public abstract class ScriptFilesProcessorStepBase : BuildStepBase
    {
        IScriptCollector _scriptCollector;

        public ScriptFilesProcessorStepBase(ILogger logger, IConfig config, ISqlServer databaseServer, IScriptCollector scriptCollector)
            : base(config, logger, databaseServer)
        {
            _scriptCollector = scriptCollector;
            ScriptPrefix = "Schema";
        }

        public string ScriptPrefix { get; set; }

        public override BuildStepResult ExecuteBuildStep()
        {
            var result = new BuildStepResult(true, null);
            try
            {
                SQLServer.EnsureConnection();
                SQLServer.DatabaseServer.ConnectionContext.BeginTransaction();

                var schemaVersionResult = SQLServer.DatabaseServer.ConnectionContext.ExecuteScalar(string.Format("select top 1 Current{0}Version from [ola_tran].[dbo].[DBVersion]", ScriptPrefix));
                int schemaVersion = 0;
                try
                {
                    schemaVersion = Convert.ToInt32(schemaVersionResult);
                }
                catch
                {
                    schemaVersion = 0;
                }
                Logger.LogMessage(string.Format("Current {0} Version in database: [{1}]", ScriptPrefix, schemaVersion), true);

                var db = SQLServer.DatabaseServer.Databases[Configuration.DatabaseName];
                var scripts = _scriptCollector.GetListOfScripts(Configuration.ScriptDirectory, ScriptPrefix);
                if (scripts.Scripts.Count > 0)
                {
                    SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(string.Format("USE [{0}]", Configuration.DatabaseName));
                    if (schemaVersion < scripts.LastSequenceNumberUsed)
                    {
                        Logger.LogMessage("{0} scripts will database from old version [{1}] to new version: [{2}]", ScriptPrefix, schemaVersion, scripts.LastSequenceNumberUsed);
                        foreach (var script in scripts.Scripts)
                        {
                            if (script.Key > schemaVersion)
                            {
                                Logger.LogMessage("About to execute {0} script '{1}'", ScriptPrefix, script.Value.Filename);
                                var count = SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(script.Value.Contents);
                                Logger.LogMessage("{0} script '{1}' executed. {2} items affected.", ScriptPrefix, script.Value.Filename, count);
                            }
                            else
                            {
                                Logger.LogMessage("Skipping {0} script '{1}' as script version of {2} is less than schema version of {3}", ScriptPrefix, script.Value.Filename, script.Key, schemaVersion);
                            }
                        }

                        var versionUpdateQuery = string.Format("update [{0}].[dbo].[{1}] set Current{2}Version={3},LastUpdated= GETDATE()",
                                                                    Configuration.DatabaseName, Configuration.VersionTableName, ScriptPrefix, scripts.LastSequenceNumberUsed);
                        SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(versionUpdateQuery);
                        SQLServer.DatabaseServer.ConnectionContext.CommitTransaction();
                    }
                    else
                    {
                        Logger.LogMessage("No {0} update will be performed as Schema version of [{1}] in database is up to date.", ScriptPrefix, schemaVersion);
                    }
                }
                else
                {
                    Logger.LogMessage("No {0} scripts were found in {1}. No schema updates performed.", ScriptPrefix, Configuration.ScriptDirectory);
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
            get { return string.Format("{0} Scripts Execution", ScriptPrefix); }
        }
    }
}
