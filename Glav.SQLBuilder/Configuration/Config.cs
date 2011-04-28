using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Configuration
{
    public class Config: IConfig
    {
		private SqlBuildMode _buildMode = ConfigPovider.Default.SqlBuildMode.ToBuildMode();
		private bool _packageOnly = ConfigPovider.Default.PackageOnly;

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

		public string MainDatabaseName
        {
			get { return ConfigPovider.Default.MainDatabaseName; }
		}

		public string[] SupportingDatabaseNames
		{
			get
			{
				if (string.IsNullOrWhiteSpace(ConfigPovider.Default.SupprtingDatabaseNames))
					return new string[] { string.Empty };

				return ConfigPovider.Default.SupprtingDatabaseNames.Split(';');
			}
        }

        public string VersionTableName
        {
            get { return ConfigPovider.Default.VersionTableName; }
        }

		public SqlBuildMode BuildMode
		{
			get { return _buildMode; }
			set { _buildMode = value; }
		}

		public Dictionary<string, string> ScriptSubstitutionsStaging
		{
			get
			{
				ConfigKeyValueParser parser = new ConfigKeyValueParser(ConfigPovider.Default.ScriptSubstitutionsStaging);
				return parser.GetKeyValues();
			}
		}
		public Dictionary<string, string> ScriptSubstitutionsRelease
		{
			get
			{
				ConfigKeyValueParser parser =new ConfigKeyValueParser(ConfigPovider.Default.ScriptSubstitutionsRelease);
				return parser.GetKeyValues();
			}
		}

		public bool PackageOnly
		{
			get { return _packageOnly;  }
			set
			{
				_packageOnly = value; 
			}
		}
    }
}
