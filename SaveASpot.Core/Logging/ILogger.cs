using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveASpot.Core.Logging
{
	public interface ILogger
	{
		void Log(ILogEntry logEntry);
	}
}
