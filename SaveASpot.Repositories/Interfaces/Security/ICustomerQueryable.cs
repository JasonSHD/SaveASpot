using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerQueryable : IElementQueryable<SiteCustomer, ICustomerFilter>
	{
		ICustomerFilter FilterByUserId(IElementIdentity identity);
		ICustomerFilter All();
	}
}