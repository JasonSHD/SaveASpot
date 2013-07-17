using System;

namespace SaveASpot.Core.Logging
{
	public interface ILogEntry
	{
		string Message { get; }
		Exception Exception { get; }
	}
}
