using System;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Phase
	{
		public ObjectId Id { get; set; }
		//public string Identity { get { return Id.ToString(); } }
		public string PhaseName { get; set; }
		public Decimal? SpotPrice { get; set; }
	}
}
