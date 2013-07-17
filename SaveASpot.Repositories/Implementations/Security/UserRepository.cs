using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserRepository : IUserRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public UserRepository(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public SiteUser CreateUser(SiteUser siteUser)
		{
			siteUser.Id = ObjectId.GenerateNewId();
			_mongoDBCollectionFactory.Collection<SiteUser>().Insert(siteUser);

			return siteUser;
		}

		public bool UpdateUserPassword(string username, string password)
		{
			var result = _mongoDBCollectionFactory.Collection<SiteUser>()
															 .Update(Query<SiteUser>.EQ(e => e.Username, username),
																			 Update<SiteUser>.Set(e => e.Password, password));

			return result.DocumentsAffected == 1;
		}
	}
}
