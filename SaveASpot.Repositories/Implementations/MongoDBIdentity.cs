using MongoDB.Bson;
using SaveASpot.Core;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class MongoDBIdentity : IElementIdentity
	{
		private readonly ObjectId _objectId;
		public ObjectId Identity { get { return _objectId; } }

		public MongoDBIdentity(ObjectId objectId)
		{
			_objectId = objectId;
		}
	}
}