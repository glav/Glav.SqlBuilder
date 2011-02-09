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
    }
}
