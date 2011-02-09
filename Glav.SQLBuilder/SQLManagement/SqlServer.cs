using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Smo;
using Glav.SQLBuilder.Logging;
using Glav.SQLBuilder.Configuration;
using Microsoft.SqlServer.Management.Common;

namespace Glav.SQLBuilder.SQLManagement
{
    public class SqlServer : ISqlServer
    {
        Server _databaseServer;
        ILogger _logger;
        IConfig _config;

        public SqlServer(IConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }

        public Server DatabaseServer { get { return _databaseServer; } }

        public bool EnsureConnection()
        {
            bool connectedOk;
            try
            {
                if (_databaseServer == null)
                {
                    _logger.LogMessage("Connecting to Database [{0}]", _config.ServerName);
                    ServerConnection serverConnection;
                    if (string.IsNullOrWhiteSpace(_config.Username) || string.IsNullOrWhiteSpace(_config.Password))
                        serverConnection = new ServerConnection(_config.ServerName);
                    else
                        serverConnection = new ServerConnection(_config.ServerName, _config.Username, _config.Password);

                    _databaseServer = new Server(serverConnection);
                }
                connectedOk = true;
            }
            catch (Exception ex)
            {
                _logger.LogMessage("Connection Failed. [{0}]", ex.Message);
                connectedOk = false;
            }
            return connectedOk;
        }

    }
}
