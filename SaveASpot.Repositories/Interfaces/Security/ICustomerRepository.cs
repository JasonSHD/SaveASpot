using MongoDB.Bson;
using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerRepository
	{
		bool AddSpot(IElementIdentity customerId, IElementIdentity spotId);
		bool CreateCustomer(IElementIdentity userIdentity);
		SiteCustomer GetCustomerByUserId(string userId);
		SiteCustomer GetCustomerById(string id);
		bool AddSpotToCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity);
		bool RemoveSpotFromCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity);
		bool UpdateSiteCustomer(string id, string stripeUserToken);
		bool UpdateCustomerCart(string customerId, ObjectId[] cartIdentity);
	}
}