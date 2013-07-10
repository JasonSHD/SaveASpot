using MongoDB.Driver;

namespace SaveASpot.Repositories.Implementations
{
	public interface IMongoDBCollectionFactory
	{
		MongoCollection<T> Collection<T>();
	}
}