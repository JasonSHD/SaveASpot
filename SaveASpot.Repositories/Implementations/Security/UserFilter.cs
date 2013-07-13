using MongoDB.Driver;
using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserFilter : IUserFilter
	{
		private readonly IMongoQuery _mongoQuery;
		public IMongoQuery MongoQuery { get { return _mongoQuery; } }

		public UserFilter(IMongoQuery mongoQuery)
		{
			_mongoQuery = mongoQuery;
		}
	}
}