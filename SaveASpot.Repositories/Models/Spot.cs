using System.Collections.Generic;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Spot
	{
		public ObjectId Id { get; set; }
		public decimal SpotLength { get; set; }
		public decimal SpotArea { get; set; }
		public List<Point> SpotShape { get; set; } 
	}
}
