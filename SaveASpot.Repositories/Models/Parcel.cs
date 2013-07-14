using System.Collections.Generic;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Parcel
	{
		public ObjectId Id { get; set; }
		public string ParcelName { get; set; }
		public decimal ParcelLength { get; set; }
		public decimal ParcelAcres { get; set; }
		public decimal ParcelArea { get; set; }
		public List<Point> ParcelShape { get; set; } 
	}
}
