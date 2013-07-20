using System.Collections.Generic;

namespace SaveASpot.Core.Configuration
{
	public interface ILogConfiguration
	{
		IEnumerable<string> GetLogLevels();
	}
}
