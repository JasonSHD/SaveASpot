using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class Cart
	{
		private ObjectId[] _spotIdCollection;

		public ObjectId[] SpotIdCollection { get { return _spotIdCollection ?? new ObjectId[0]; } set { _spotIdCollection = value; } }
	}
}