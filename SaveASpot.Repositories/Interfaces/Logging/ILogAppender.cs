using System;

namespace SaveASpot.Repositories.Interfaces.Logging
{
	public interface ILogAppender
	{
		void Log(string level, string message, Exception exception);
	}
}
