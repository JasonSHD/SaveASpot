using SaveASpot.Core;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerRepository
	{
		bool AddSpot(IElementIdentity customerId, IElementIdentity spotId);
		IElementIdentity CreateCustomer(IElementIdentity userIdentity);
		bool UpdateSiteCustomer(IElementIdentity id, string stripeUserToken);
	}
}