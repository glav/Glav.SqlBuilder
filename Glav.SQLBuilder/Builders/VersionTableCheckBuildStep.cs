using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;
using Glav.SQLBuilder.Helpers;
using Microsoft.SqlServer.Management.Smo;

namespace Glav.SQLBuilder.Builders
{
    public class VersionTableCheckBuildStep : BuildStepBase
    {
        public VersionTableCheckBuildStep(ILogger logger, IConfig config, ISqlServer databaseServer) : base(config, logger, databaseServer) { }

        public override BuildStepResult ExecuteBuildStep()
        {
            var result = new BuildStepResult(true, null);
            try
            {
                SQLServer.EnsureConnection();
                var db = SQLServer.DatabaseServer.Databases[Configuration.DatabaseName];
                if (db.Tables.Contains(Configuration.VersionTableName))
                {
                    Logger.LogMessage("Version Table [{0}] already exists, no need to create.",Configuration.VersionTableName);
                }
                else
                {
                    var tableToCreate = new Table(db, Configuration.VersionTableName);

                    tableToCreate.Columns.Add(new Column(tableToCreate, "CurrentSchemaVersion", DataType.Int));
                    tableToCreate.Columns.Add(new Column(tableToCreate, "CurrentDataVersion", DataType.Int));
                    tableToCreate.Columns.Add(new Column(tableToCreate, "LastUpdated", DataType.DateTime));
                    tableToCreate.Create();
                    SQLServer.DatabaseServer.ConnectionContext.ExecuteNonQuery(string.Format("insert into [{0}].[dbo].[{1}] (CurrentSchemaVersion,CurrentDataVersion,LastUpdated) values (0,0,'20000101')", Configuration.DatabaseName, Configuration.VersionTableName));
                    SQLServer.DatabaseServer.ConnectionContext.CommitTransaction();

                    Logger.LogMessage("Version Table [{0}] created successfully.", Configuration.VersionTableName);

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
            get { return "Version Table Check"; }
        }

    }
}
