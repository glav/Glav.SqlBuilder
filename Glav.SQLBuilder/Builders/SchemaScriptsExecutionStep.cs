using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.SQLManagement;
using Glav.SQLBuilder.Helpers;

namespace Glav.SQLBuilder.Builders
{
    public class SchemaScriptsExecutionStep : ScriptFilesProcessorStepBase
    {
        public SchemaScriptsExecutionStep(ILogger logger, IConfig config, ISqlServer databaseServer, IScriptCollector scriptCollector)
            : base(logger,config, databaseServer, scriptCollector)
        {
            ScriptPrefix = "Schema";
        }

    }
}
