using System.Collections.Generic;
using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.PhasesAndParcels
{
	public interface ISpotsService
	{
		bool RemoveSpots(IEnumerable<IElementIdentity> spotIdentityCollection);
		IMethodResult<MessageResult> AddSpots(StreamReader streamReader);
	}
}