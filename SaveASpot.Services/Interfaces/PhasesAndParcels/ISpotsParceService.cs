using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.PhasesAndParcels
{
	public interface ISpotsParceService
	{
		IMethodResult<MessageResult> AddSpots(StreamReader input);
	}
}