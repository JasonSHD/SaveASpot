using System.Collections.Generic;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerQueryable
	{
		ICustomerFilter FilterByUserId(string identity);
		IEnumerable<SiteCustomer> Find(ICustomerFilter customerFilter);
	}
}