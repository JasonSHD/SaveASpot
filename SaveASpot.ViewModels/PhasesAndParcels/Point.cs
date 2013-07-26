namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class Point
	{
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }

		public object ToJson()
		{
			return new { lat = Latitude, lng = Longitude };
		}
	}
}