using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Glav.SQLBuilder.Configuration
{
	public enum SqlBuildMode
	{
		Normal,
		Staging,
		Release
	}

	internal static class SqlBuildModeValues
	{
		public const string Normal = "normal";
		public const string Staging = "staging";
		public const string Release = "release";
	}

	public static class SqlBuildModeExtensions
	{
		public static SqlBuildMode ToBuildMode(this string modeText)
		{
			if (string.IsNullOrWhiteSpace((modeText)))
				return SqlBuildMode.Normal;

			var normalisedText = modeText.ToLowerInvariant();
			if (normalisedText == SqlBuildModeValues.Staging)
				return SqlBuildMode.Staging;

			if (normalisedText == SqlBuildModeValues.Release)
				return SqlBuildMode.Release;

			return SqlBuildMode.Normal;
		}
	}
}
