using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Logging
{
    public interface ILogger
    {
        void LogMessage(string message, bool outputToConsole);
        void LogMessage(string message);
        void LogMessage(string formatMessage, params object[] args);
    }
}
