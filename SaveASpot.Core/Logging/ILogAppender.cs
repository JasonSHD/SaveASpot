using System;

namespace SaveASpot.Core.Logging
{
	public interface ILogAppender
	{
		void Log(string level, string message, Exception exception);
	}
}
