using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerQueryable : IElementQueryable<SiteCustomer, ICustomerFilter>
	{
		ICustomerFilter FilterByUserId(string identity);
		ICustomerFilter All();
	}
}