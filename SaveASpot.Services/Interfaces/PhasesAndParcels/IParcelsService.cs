using System.Collections.Generic;
using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.PhasesAndParcels
{
	public interface IParcelsService
	{
		bool RemoveParcels(IEnumerable<IElementIdentity> parcelIdentity);
		IMethodResult<MessageResult> AddParcels(StreamReader input, decimal spotPrice);
	}
}