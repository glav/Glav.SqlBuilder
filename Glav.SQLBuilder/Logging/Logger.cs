using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Logging
{
    public class Logger : ILogger
    {
        public void LogMessage(string message)
        {
            LogMessage(message, true);
        }

        public void LogMessage(string formatMessage, params object[] args)
        {
            LogMessage(string.Format(formatMessage, args), true);
        }

        public void LogMessage(string message, bool outputToConsole)
        {
            //TODO: Output to file, may useLog4Net or whatever
            string logMsg =string.Format("[{0}] {1}",DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),message);
            System.Diagnostics.Trace.WriteLine(logMsg);
            if (outputToConsole)
                Console.WriteLine(logMsg);
        }
    }
}
