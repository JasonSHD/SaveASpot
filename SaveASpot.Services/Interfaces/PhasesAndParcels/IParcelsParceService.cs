using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.PhasesAndParcels
{
	public interface IParcelsParceService
	{
		IMethodResult<MessageResult> AddParcels(StreamReader input, decimal spotPrice);
	}
}