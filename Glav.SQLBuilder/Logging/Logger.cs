using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Logging
{
    public class Logger : ILogger
    {

        public void LogMessage(string message, bool outputToConsole)
        {
            string logMsg = FormatMessage(message);
            System.Diagnostics.Trace.WriteLine(logMsg);
            if (outputToConsole)
                Console.WriteLine(logMsg);
        }

        public void LogMessage(string message)
        {
            LogMessage(message, true);
        }

        public void LogMessage(string messageFormat, params object[] args)
        {
            LogMessage(string.Format(messageFormat, args),true);

        }

        private string FormatMessage(string message)
        {
            return string.Format("[{0}] {1}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"), message);
        }
    }
}
