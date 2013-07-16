using System;

namespace SaveASpot.Core.Logging.Interfaces
{
	public interface ILogAppender
	{
		void Log(string level, string message, Exception exception);
	}
}
