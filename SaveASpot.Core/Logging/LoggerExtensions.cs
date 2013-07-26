using System;
using SaveASpot.Core.Logging.Implementation;

namespace SaveASpot.Core.Logging
{
	public static class LoggerExtensions
	{
		public static void Info(this ILogger logger, string message)
		{
			logger.Log(new InfoLogEntry(message));
		}

		public static void Info(this ILogger logger, string message, params object[] args)
		{
			logger.Log(new InfoLogEntry(string.Format(message, args)));
		}

		public static void Error(this ILogger logger, string message, Exception exception)
		{
			logger.Log(new ErrorLogEntry(exception, message));
		}

		public static void JavaScriptError(this ILogger logger, string message)
		{
			logger.Log(new ErrorLogEntry(null, message));
		}
	}
}
