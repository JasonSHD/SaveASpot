using System;

namespace SaveASpot.Core.Logging.Interfaces
{
	public interface ILogEntry
	{
		string Message { get; }
		Exception Exception { get; }
	}
}
