namespace SaveASpot.Core.Geocoding
{
	public sealed class Point
	{
		public decimal Latitude { get; set; } //широта(y)
		public decimal Longitude { get; set; } //долгота(x)

		public static bool PointInSquare(Point source, Point leftBottom, Point rightTop)
		{
			return leftBottom.Latitude <= source.Longitude && leftBottom.Longitude <= source.Latitude &&
						 rightTop.Latitude >= source.Longitude && rightTop.Longitude >= source.Latitude;

			return leftBottom.Longitude <= source.Longitude && leftBottom.Latitude <= source.Latitude &&
						 rightTop.Longitude >= source.Longitude && rightTop.Latitude >= source.Latitude;
		}
	}
}
