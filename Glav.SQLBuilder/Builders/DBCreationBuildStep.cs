using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;
using Microsoft.SqlServer.Management.Smo;
using Glav.SQLBuilder.Helpers;

namespace Glav.SQLBuilder.Builders
{
    public class DBCreationBuildStep : BuildStepBase
    {
        public DBCreationBuildStep(ILogger logger, IConfig config, ISqlServer databaseServer) : base(config,logger, databaseServer){}

        public override BuildStepResult ExecuteBuildStep()
        {
            var result = new BuildStepResult(true, null);
            try
            {
                SQLServer.EnsureConnection();
                if (SQLServer.DatabaseServer.Databases.Contains(Configuration.DatabaseName))
                {
                    Logger.LogMessage("Database {0} already exists. No need to create.", Configuration.DatabaseName);
                }
                else
                {
                    Logger.LogMessage("Database {0} not found. Attempting to create.", Configuration.DatabaseName);
                    var dbToCreate = new Database(SQLServer.DatabaseServer, Configuration.DatabaseName);
                    dbToCreate.Create();
                    SQLServer.DatabaseServer.Databases.Refresh();
                    SQLServer.DatabaseServer.ConnectionContext.BeginTransaction();
                    if (SQLServer.DatabaseServer.Databases.Contains(Configuration.DatabaseName))
                        Logger.LogMessage("Database {0} was created successfully. Transaction context started.", Configuration.DatabaseName);
                    else
                    {
                        Logger.LogMessage("Database {0} could not be created.", Configuration.DatabaseName);
                        result.Successful = false;
                        result.CanContinue = false;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.CanContinue = false;
                result.ResultDetail = ex.GetFullExceptionMessages();
            }

            return result;
        }

        public override string Name
        {
            get { return "Database Creation"; }
        }
    }
}
