using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class Cart
	{
		public ObjectId Id { get; set; }

		private ObjectId[] _spotIdCollection;

		public ObjectId[] SpotIdCollection { get { return _spotIdCollection ?? new ObjectId[0]; } set { _spotIdCollection = value; } }
	}
}