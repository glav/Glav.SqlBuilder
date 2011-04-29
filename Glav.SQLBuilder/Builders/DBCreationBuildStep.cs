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
		public DBCreationBuildStep(ILogger logger, IConfig config, ISqlServer databaseServer) : base(config, logger, databaseServer) { }

		public override BuildStepResult ExecuteBuildStep()
		{
			var result = new BuildStepResult(true, null);
			try
			{
				SQLServer.EnsureConnection();

				// Try and create the main database first. Without it, nothing works. Then
				// we try and create any supporting databases.
				bool creationOk = CreateDatabase(Configuration.MainDatabaseName);
				if (creationOk)
				{
					Logger.LogMessage("Main Database {0} created successfully. Attempting to create supporting databases (if any specified)", Configuration.MainDatabaseName);
					if (Configuration.SupportingDatabaseNames.Length > 0)
					{
						foreach (var db in Configuration.SupportingDatabaseNames)
						{
							if (!string.IsNullOrEmpty(db))
							{
								creationOk = CreateDatabase(db);
								if (!creationOk)
								{
									Logger.LogMessage("Supporting database {0} was NOT created successfully.", db);
									break;
								}
								else
								{
									if (!Configuration.PackageOnly)
									{
										Logger.LogMessage("Supporting database {0} created successfully.", db);
									}
								}
							}
						}
					}

				}

				if (creationOk)
				{
					Logger.LogMessage("All database were created successfully.");
					result.Successful = true;
					result.CanContinue = true;
				}
				else
				{
					result.Successful = false;
					result.CanContinue = false;
					result.ResultDetail = "Unable to create necessary databases.";
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

		private bool CreateDatabase(string databaseName)
		{
			if (Configuration.PackageOnly)
			{
				Logger.LogMessage("In PackageaOnly Mode. No need to create.", databaseName);
				return true;
			}

			if (SQLServer.DatabaseServer.Databases.Contains(databaseName))
			{
				Logger.LogMessage("Database {0} already exists. No need to create.", databaseName);
				return true;
			}
			Logger.LogMessage("Database {0} not found. Attempting to create.", databaseName);
			var dbToCreate = new Database(SQLServer.DatabaseServer, databaseName);
			dbToCreate.Create();
			SQLServer.DatabaseServer.Databases.Refresh();
			return SQLServer.DatabaseServer.Databases.Contains(databaseName);
		}
	}
}
