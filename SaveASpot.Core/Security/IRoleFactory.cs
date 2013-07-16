using System;

namespace SaveASpot.Core.Security
{
	public interface IRoleFactory
	{
		Role Convert(string identity);
		Role Convert(Type role);
	}
}