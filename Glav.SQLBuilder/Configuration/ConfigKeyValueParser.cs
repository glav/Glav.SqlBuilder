using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Configuration
{
	public class ConfigKeyValueParser
	{
		private string _configValue;

		public ConfigKeyValueParser(string configValue)
		{
			_configValue = configValue;
		}

		public Dictionary<string, string> GetKeyValues()
		{
			Dictionary<string, string> list = new Dictionary<string, string>();
			if (!string.IsNullOrWhiteSpace(_configValue))
			{
				var arrayOfEntries = _configValue.Split(';');
				if (arrayOfEntries.Length > 0)
				{
					arrayOfEntries
						.ToList()
						.ForEach(a =>
						         	{
						         		var keyValue = a.Split('=');
						         		if (keyValue != null && keyValue.Length == 2)
						         		{
						         			list.Add(keyValue[0], keyValue[1]);
						         		}
						         	});
				}
			}
			return list;
		}
	}
}
