using System;
using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IRoleHarvester
	{
		Role Convert(string identity);
		Role Convert(Type role);
	}
}