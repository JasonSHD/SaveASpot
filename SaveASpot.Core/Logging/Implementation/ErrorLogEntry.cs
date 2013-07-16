using System;

namespace SaveASpot.Core.Logging.Implementation
{
	public class ErrorLogEntry : ILogEntry
	{
		private readonly string _message;
		private readonly Exception _exception;

		public ErrorLogEntry(Exception exception, string message)
		{
			_message = message == string.Empty ? exception.Message : message;
			_exception = exception;
		}
		public string Message
		{
			get { return _message; }
		}

		public Exception Exception
		{
			get { return _exception; }
		}
	}
}
