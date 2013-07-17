using System.Collections.Generic;

namespace SaveASpot.Core.Configuration
{
	public class LogConfiguration:ILogConfiguration
	{
		private readonly IConfigurationManager _configurationManager;
		private const string LOG_LEVELS = "logLevels";

		public LogConfiguration(IConfigurationManager configurationManager)
		{
			_configurationManager = configurationManager;
		}

		public IEnumerable<string> GetLogLevels()
		{
			return _configurationManager.GetSettings(LOG_LEVELS).Split(',');
		}
	}
}
