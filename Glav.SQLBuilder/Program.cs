using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Glav.SQLBuilder.Bootstrap;
using Glav.SQLBuilder.Configuration;
using Glav.SQLBuilder.Logging;
using System.Transactions;

namespace Glav.SQLBuilder
{
    class Program
    {
		private static IServiceResolver _resolver;

        static void Main(string[] args)
        {
            Logger logger = new Logger();
            logger.LogMessage("", false);
            logger.LogMessage("*************************", false);
            logger.LogMessage("Starting Saasu SQLBuilder.", true);

			_resolver = new ServiceResolver();
			AppStart appStart = new AppStart(_resolver);

			var modeParsedOk = ParseBuildModeFromCmdLineArgs(args);
			if (!modeParsedOk)
			{
				Environment.ExitCode = 0;
				return;
			}

            bool ranOk = appStart.PerformBuild();

            if (ranOk)
                Environment.ExitCode = 0;
            else
                Environment.ExitCode = 1;
            
            logger.LogMessage(string.Format("Completed Saasu SQLBuilder with Exit Code of [{0}]",Environment.ExitCode), true);
        }
		static bool ParseBuildModeFromCmdLineArgs(string[] args)
		{
			SqlBuildMode mode = SqlBuildMode.Normal;
			var logger = _resolver.Get<ILogger>();

			if (args != null && args.Length > 0)
			{
				if (args[0] == "/?" || args[0] == "?")
				{
					Console.WriteLine();
					Console.WriteLine("Saasu Sql Builder");
					Console.WriteLine("~~~~~~~~~~~~~~~~~");
					Console.WriteLine(" > Saasu.SQLBuilder /?");
					Console.WriteLine("   This help text.");
					Console.WriteLine(" > Saasu.SQLBuilder /mode: release|staging|normal");
					Console.WriteLine("   Sets the build mode to either 'Normal' (the default), 'Staging',");
					Console.WriteLine("    or 'Release'. In 'Staging' or 'Release' modes, SQLBuilder will");
					Console.WriteLine("   perform string substitution on all scripts based on the defined");
					Console.WriteLine("   substitution settings on the config file, before executing any script.");
					Console.WriteLine("   For example, in 'Release' mode, all instances of 'ola_tran'could be");
					Console.WriteLine("   replaced with 'ola_live'.");
					Console.WriteLine(" > Saasu.SQLBuilder /packageOnly");
					Console.WriteLine("   Will only perform string substitition on the scripts to be executed");
					Console.WriteLine("   and saves those scripts to disk,but does not execute them. Files are saved");
					Console.WriteLine("    with 'Normal|Staging|Release' suffix depending onthe build mode.");
					return false;
				}

				foreach (var cmdLineOption in args)
				{
					ParseCommandLineBuildModeOption(cmdLineOption);
					ParseCommandLinePackageOption(cmdLineOption);
				}
			}

			logger.LogMessage("Build Mode: {0}", mode.ToString());
			return true;
		}

		private static void ParseCommandLinePackageOption(string cmdLineOption)
		{
			var config = _resolver.Get<IConfig>();
			if (string.IsNullOrWhiteSpace(cmdLineOption))
				return;

			if (cmdLineOption.ToLowerInvariant() == "/packageonly")
			{
				config.PackageOnly = true;
			}
		}

		private static void ParseCommandLineBuildModeOption(string cmdLineOption)
		{
			var config = _resolver.Get<IConfig>();
			if (string.IsNullOrWhiteSpace(cmdLineOption))
				return;

			var splitOption = cmdLineOption.Split(':');
			if (splitOption.Length == 2)
			{
				var setting = splitOption[0].ToLowerInvariant();
				if (setting == "/mode")
				{
					var settingValue = splitOption[1].ToLowerInvariant();
					config.BuildMode = settingValue.ToBuildMode();
				}
			}
		}

    }
}
