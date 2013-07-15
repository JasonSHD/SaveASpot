using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Phase
	{
		public ObjectId Id { get; set; }
		public string PhaseName { get; set; }
	}
}
