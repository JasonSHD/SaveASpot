using System;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public static class UserQueryableExtensions
	{
		public static QueryableBuilder<SiteUser, IUserFilter, IUserQueryable> Filter(this IUserQueryable source, Func<IUserQueryable, IUserFilter> filter)
		{
			return new QueryableBuilder<SiteUser, IUserFilter, IUserQueryable>(source).And(filter);
		}
	}
}