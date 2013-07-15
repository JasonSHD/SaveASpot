using System;

namespace SaveASpot.Core.Security
{
	public interface IRoleHarvester
	{
		Role Convert(string identity);
		Role Convert(Type role);
	}
}