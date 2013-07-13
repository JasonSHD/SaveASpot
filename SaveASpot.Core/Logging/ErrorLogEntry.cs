using System;

namespace SaveASpot.Core.Logging
{
	public class ErrorLogEntry : ILogEntry
	{
		private readonly string _message;
		private readonly Exception _exception;

		public ErrorLogEntry(Exception exception)
		{
			_message = exception.Message;
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
