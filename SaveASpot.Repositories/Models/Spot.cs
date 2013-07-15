using System.Collections.Generic;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Spot
	{
		public ObjectId Id { get; set; }
		public decimal Length { get; set; }
		public decimal Area { get; set; }
		public List<Point> Shape { get; set; } 
	}
}
