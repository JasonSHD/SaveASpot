using MongoDB.Bson;
using SaveASpot.Core;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class MongoDBElementIdentityConverter : IElementIdentityConverter
	{
		public IElementIdentity ToIdentity(string identity)
		{
			ObjectId objectId;
			ObjectId.TryParse(identity, out objectId);

			return objectId == ObjectId.Empty ? (IElementIdentity)new NullElementIdentity() : new MongoDBIdentity(objectId);
		}

		public IElementIdentity ToIdentity(object identity)
		{
			var isObjectId = identity is ObjectId;
			if (isObjectId)
			{
				return new MongoDBIdentity((ObjectId)identity);
			}

			var identityAsString = identity.ToString();
			ObjectId objectId;
			ObjectId.TryParse(identityAsString, out objectId);
			return objectId == ObjectId.Empty ? (IElementIdentity)new NullElementIdentity() : new MongoDBIdentity(objectId);
		}
	}
}