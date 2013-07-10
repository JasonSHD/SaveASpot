using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserQueryable : IUserQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public UserQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public IUserFilter FilterByName(string name)
		{
			return ToFilter(e => e.Username == name);
		}

		public IUserFilter FiltreByPassword(string password)
		{
			return ToFilter(e => e.Password == password);
		}

		public IUserFilter FilterById(string identity)
		{
			ObjectId objectId;
			if (ObjectId.TryParse(identity, out objectId))
			{
				return ToFilter(e => e.Id == objectId);
			}

			return ToFilter(e => false);
		}

		public IUserFilter And(IUserFilter first, IUserFilter second)
		{
			return new UserFilter(e => ToFilter(second).Filter(ToFilter(first).Filter(e)));
		}

		public IEnumerable<UserEntity> FindUsers(IUserFilter userFilter)
		{
			return ToFilter(userFilter).Filter(_mongoDBCollectionFactory.Collection<UserEntity>().AsQueryable());
		}

		private IUserFilter ToFilter(Expression<Func<UserEntity, bool>> expression)
		{
			return new UserFilter(e => e.Where(expression));
		}

		private UserFilter ToFilter(IUserFilter userFilter)
		{
			if (userFilter == null || userFilter.GetType() != typeof(UserFilter))
				throw new ArgumentException();

			return (UserFilter)userFilter;
		}
	}
}