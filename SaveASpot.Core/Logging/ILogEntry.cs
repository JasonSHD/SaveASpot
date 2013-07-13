using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Core.Logging
{
	public interface ILogEntry
	{
		string Message { get; }
		Exception Exception { get; }
		//string StackTrace { get; }
		//string InnerException { get; }
	}
}
