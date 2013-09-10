using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerRepository
	{
		bool AddSpot(IElementIdentity customerId, IElementIdentity spotId);
		IElementIdentity CreateCustomer(IElementIdentity userIdentity);
		SiteCustomer GetCustomerByUserId(string userId);
		SiteCustomer GetCustomerById(string id);
		bool UpdateSiteCustomer(string id, string stripeUserToken);
	}
}