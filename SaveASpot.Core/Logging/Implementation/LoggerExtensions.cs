using System;
using SaveASpot.Core.Logging.Interfaces;

namespace SaveASpot.Core.Logging.Implementation
{
	public static class LoggerExtensions
	{
		public static void Info(this ILogger logger, string message)
		{
			logger.Log(new InfoLogEntry(message));
		}

		public static void Error(this ILogger logger, string message, Exception exception)
		{
			logger.Log(new ErrorLogEntry(exception, message));
		}
	}
}
