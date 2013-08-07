using MongoDB.Bson;

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
	}
}