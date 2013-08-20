using SaveASpot.Core;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerRepository
	{
		bool AddSpot(IElementIdentity customerId, IElementIdentity spotId);
		bool CreateCustomer(IElementIdentity userIdentity);
		bool AddSpotToCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity);
		bool RemoveSpotFromCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity);
	}
}