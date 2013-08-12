using MongoDB.Bson;
using SaveASpot.Core;

namespace SaveASpot.Repositories.Implementations
{
	public static class MongoObjectIdExtensions
	{
		public static ObjectId ToIdentity(this string source)
		{
			ObjectId objectId;
			ObjectId.TryParse(source, out objectId);
			return objectId;
		}

		public static ObjectId ToIdentity(this IElementIdentity elementIdentity)
		{
			var mongoDbIdentity = elementIdentity as MongoDBIdentity;
			if (mongoDbIdentity != null)
			{
				return mongoDbIdentity.Identity;
			}

			var identityAsString = elementIdentity.ToString();
			ObjectId identityResult;
			ObjectId.TryParse(identityAsString, out identityResult);

			return identityResult;
		}
	}
}