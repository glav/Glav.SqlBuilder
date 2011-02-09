using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Bootstrap;
using Glav.SQLBuilder.Logging;
using System.Transactions;

namespace Glav.SQLBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();
            logger.LogMessage("", false);
            logger.LogMessage("*************************", false);
            logger.LogMessage("Starting Saasu SQLBuilder.", true);

            IServiceResolver resolver = new ServiceResolver();
            AppStart appStart = new AppStart(resolver);
            bool ranOk = appStart.PerformBuild();

            if (ranOk)
                Environment.ExitCode = 0;
            else
                Environment.ExitCode = 1;
            
            logger.LogMessage(string.Format("Completed Saasu SQLBuilder with Exit Code of [{0}]",Environment.ExitCode), true);
        }
    }
}
