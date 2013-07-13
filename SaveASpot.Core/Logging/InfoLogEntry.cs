using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Core.Logging
{
	public class InfoLogEntry : ILogEntry
	{
		private string _message;

		public InfoLogEntry(string message)
		{
			_message = message;
		}

		public string Message
		{
			get { return _message; }
		}

		public Exception Exception
		{
			get { return null; }
		}

	}
}
