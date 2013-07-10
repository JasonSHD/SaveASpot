using System;
using System.Linq;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class UserFilter : IUserFilter
	{
		private readonly Func<IQueryable<UserEntity>, IQueryable<UserEntity>> _filter;

		public UserFilter(Func<IQueryable<UserEntity>, IQueryable<UserEntity>> filter)
		{
			_filter = filter;
		}

		public IQueryable<UserEntity> Filter(IQueryable<UserEntity> queryable)
		{
			return _filter(queryable);
		}
	}
}