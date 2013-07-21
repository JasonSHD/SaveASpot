using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface IArcgisService
	{
        IMethodResult<MessageResult> AddParcels(string jsonParcels);
        IMethodResult<MessageResult> AddSpots(string jsonSpots);
	}
}
