using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Implementations
{
	public sealed class Polygon
	{
		private readonly Point[] _vertices;
		public Polygon(Point[] vertices)
		{
			_vertices = vertices;
		}

		/// <summary>  
		///     Determines if the specified <see cref="Point"/> if within this polygon.  
		/// </summary>  
		/// <param name="point">  
		///     The point containing the x(Longitude), y(Latitude) coordinates to check.  
		/// </param>  
		public bool PointInPolygon(Point point)
		{
			bool isInside = false;
			for (int i = 0, j = _vertices.Length - 1; i < _vertices.Length; j = i++)
			{
				//check if this point is between Y coordinates of polygon
				if (_vertices[i].Latitude < point.Latitude && _vertices[j].Latitude >= point.Latitude ||
						_vertices[j].Latitude < point.Latitude && _vertices[i].Latitude >= point.Latitude)
				{
					//if it is between Y coordinates, check if point is beetween X coordinates of this polygon
					if (_vertices[i].Longitude + (point.Latitude - _vertices[i].Latitude) / (_vertices[j].Latitude - _vertices[i].Latitude) * (_vertices[j].Longitude - _vertices[i].Longitude) < point.Longitude)
					{
						isInside = !isInside;
					}
				}
			}
			return isInside;
		}
	}
}