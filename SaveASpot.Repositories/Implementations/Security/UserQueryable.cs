using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserQueryable : BasicMongoDBElementQueryable<SiteUser, IUserFilter>, IUserQueryable
	{
		public UserQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory) : base(mongoDBCollectionFactory) { }

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

		public IUserFilter FilterByIds(string[] identities)
		{
			return new UserFilter(Query.Null);
		}

		protected override IUserFilter BuildFilter(IMongoQuery query)
		{
			return new UserFilter(query);
		}

		private IUserFilter ToFilter(Expression<Func<SiteUser, bool>> expression)
		{
			return new UserFilter(Query<SiteUser>.Where(expression));
		}
	}
}