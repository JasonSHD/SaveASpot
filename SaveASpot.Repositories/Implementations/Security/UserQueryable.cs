using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Core.Security;
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

		public IUserFilter FilterByPassword(string password)
		{
			return ToFilter(e => e.Password == password);
		}

		public IUserFilter FilterByRole(Role roleFilter)
		{
			var roleIdentity = roleFilter.Identity;
			var query = Query<SiteUser>.EQ(e => e.Roles, new[] { roleIdentity });
			return new UserFilter(query);
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
			return new UserFilter(Query.And(new[] { ToFilter(first).MongoQuery, ToFilter(second).MongoQuery }));
		}

		public IEnumerable<SiteUser> FindUsers(IUserFilter userFilter)
		{
			return _mongoDBCollectionFactory.Collection<SiteUser>().Find(ToFilter(userFilter).MongoQuery);
		}

		private IUserFilter ToFilter(Expression<Func<SiteUser, bool>> expression)
		{
			return new UserFilter(Query<SiteUser>.Where(expression));
		}

		private UserFilter ToFilter(IUserFilter userFilter)
		{
			if (userFilter == null || userFilter.GetType() != typeof(UserFilter))
				throw new ArgumentException();

			return (UserFilter)userFilter;
		}
	}
}