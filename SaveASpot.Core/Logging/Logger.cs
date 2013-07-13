using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Configuration;
using SaveASpot.Repositories.Interfaces.Logging;


namespace SaveASpot.Core.Logging
{
	public class Logger : ILogger
	{
		private readonly IEnumerable<ILogAppender> _logAppenders;
		private readonly ILogConfiguration _logConfiguration;

		public Logger(IEnumerable<ILogAppender> logAppenders, ILogConfiguration logConfiguration)
		{
			_logAppenders = logAppenders;
			_logConfiguration = logConfiguration;
		}

		public void Log(ILogEntry logEntry)
		{
			if (CheckLogLevel(logEntry))
			{
				var loglevel = logEntry.GetType().Name.Replace("LogEntry", string.Empty);
				foreach (var logAppender in _logAppenders)
				{
					switch (loglevel)
					{
						case "Info":
							logAppender.Log(loglevel,logEntry.Message);
							break;
						case "Error":
							logAppender.Log(loglevel, logEntry.Exception);
							break;
					}
				}
			}
		}

		private bool CheckLogLevel(ILogEntry logEntry)
		{
			return _logConfiguration.GetLogLevels().Any(e => e.Equals(logEntry.GetType().Name.Replace("LogEntry", string.Empty), StringComparison.InvariantCultureIgnoreCase));
		}
	}
}