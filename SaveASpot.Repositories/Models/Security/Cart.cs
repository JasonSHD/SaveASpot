using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class Cart
	{
		public ObjectId[] SpotIdCollection { get; set; }
	}
}