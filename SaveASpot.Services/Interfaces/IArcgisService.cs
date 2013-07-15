namespace SaveASpot.Services.Interfaces
{
	public interface IArcgisService
	{
		void AddParcels(string jsonParcels);
		void AddSpots(string jsonSpots);
	}
}
