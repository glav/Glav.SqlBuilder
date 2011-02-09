using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;

namespace Glav.SQLBuilder.Builders
{
    public class PostBuildScriptStep : ScriptExecutionStepBase
    {
        public PostBuildScriptStep(IConfig config, ILogger logger, ISqlServer databaseServer)
            : base(logger, config, databaseServer) 
        {
            ExecutionPhase = ScriptExecutionPhase.PostBuild;
        }

        public override BuildStepResult ExecuteBuildStep()
        {
            var result = base.ExecuteBuildStep();
            SQLServer.DatabaseServer.ConnectionContext.Disconnect();
            return result;
        }
    }
}
