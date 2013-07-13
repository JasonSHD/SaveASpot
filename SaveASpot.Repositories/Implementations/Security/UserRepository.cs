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

		public UserEntity CreateUser(UserEntity userEntity)
		{
			userEntity.Id = ObjectId.GenerateNewId();
			_mongoDBCollectionFactory.Collection<UserEntity>().Insert(userEntity);

			return userEntity;
		}

		public bool UpdateUserPassword(string username, string password)
		{
			var result = _mongoDBCollectionFactory.Collection<UserEntity>()
															 .Update(Query<UserEntity>.EQ(e => e.Username, username),
																			 Update<UserEntity>.Set(e => e.Password, password));

			return result.DocumentsAffected == 1;
		}
	}
}
