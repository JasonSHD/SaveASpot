using System;
using MongoDB.Driver;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class MongoDBProvider : IMongoDBCollectionFactory
	{
		private readonly IMongoDBConfiguration _mongoDBConfiguration;

		public MongoDBProvider(IMongoDBConfiguration mongoDBConfiguration)
		{
			_mongoDBConfiguration = mongoDBConfiguration;
		}

		public MongoCollection<T> Collection<T>()
		{
			var mongoClient = new MongoClient(_mongoDBConfiguration.ConnectionString);
			return mongoClient.GetServer().GetDatabase(_mongoDBConfiguration.DatabaseName).GetCollection<T>(GetCollectionName(typeof(T)));
		}

		private string GetCollectionName(Type type)
		{
			return type.Name;
		}
	}
}
