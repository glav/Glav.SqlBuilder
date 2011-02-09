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
    public class DataScriptsExecutionStep : ScriptFilesProcessorStepBase
    {
        public DataScriptsExecutionStep(ILogger logger, IConfig config, ISqlServer databaseServer, IScriptCollector scriptCollector)
            : base(logger,config, databaseServer,scriptCollector)
        {
            ScriptPrefix = "Data";
        }

    }

}
