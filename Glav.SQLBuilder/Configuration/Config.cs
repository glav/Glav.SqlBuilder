using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Configuration
{
    public class Config: IConfig
    {
        public string ServerName
        {
            get { return ConfigPovider.Default.ServerName; }
        }

        public string Username
        {
            get { return ConfigPovider.Default.Username; }
        }

        public string Password
        {
            get { return ConfigPovider.Default.Password; }
        }

        public string ScriptDirectory
        {
            get { return ConfigPovider.Default.ScriptDirectory; }
        }

        public string PostScriptToRun
        {
            get { return ConfigPovider.Default.PostScriptToRun; }
        }

        public string PreScriptToRun 
        { 
            get { return ConfigPovider.Default.PreScriptToRun; }
        }

        public string DatabaseName
        {
            get { return ConfigPovider.Default.DatabaseName; }
        }

        public string VersionTableName
        {
            get { return ConfigPovider.Default.VersionTableName; }
        }
    }
}
