using System;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public static class CustomerQueryableExtensions
	{
		public static QueryableBuilder<SiteCustomer, ICustomerFilter, ICustomerQueryable> Filter(
			this ICustomerQueryable source, Func<ICustomerQueryable, ICustomerFilter> filter)
		{
			return new QueryableBuilder<SiteCustomer, ICustomerFilter, ICustomerQueryable>(source).And(filter);
		}
	}
}