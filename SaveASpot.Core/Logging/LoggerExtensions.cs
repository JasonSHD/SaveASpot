using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Core.Logging
{
	public static class LoggerExtensions
	{
		public static void Info(this ILogger logger, string message)
		{
			logger.Log(new InfoLogEntry(message));
		}

		public static void Error(this ILogger logger, Exception exception)
		{
			logger.Log(new ErrorLogEntry(exception));
		}
	}
}
