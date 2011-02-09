using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;
using Glav.SQLBuilder.Helpers;
using System.IO;

namespace Glav.SQLBuilder.Builders
{
    public class PreBuildScriptStep : ScriptExecutionStepBase
    {
        public PreBuildScriptStep(IConfig config,ILogger logger, ISqlServer databaseServer) : base(logger, config, databaseServer) 
        {
            ExecutionPhase = ScriptExecutionPhase.PreBuild;
        }

    }
}
