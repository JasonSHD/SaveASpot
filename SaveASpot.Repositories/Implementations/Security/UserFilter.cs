using MongoDB.Driver;
using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserFilter : MongoQueryFilter, IUserFilter
	{
		public UserFilter(IMongoQuery mongoQuery)
			: base(mongoQuery)
		{
		}
	}
}