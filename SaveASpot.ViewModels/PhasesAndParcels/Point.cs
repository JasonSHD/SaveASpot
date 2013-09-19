using SaveASpot.Core.Geocoding;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public static class PointExtensions
	{
		public static object ToJson(this Point source)
		{
			return new { lat = source.Latitude, lng = source.Longitude };
		}
	}
}