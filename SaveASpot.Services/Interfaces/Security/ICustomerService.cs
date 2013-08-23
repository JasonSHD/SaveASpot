using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface ICustomerService
	{
		IMethodResult<CreateCustomerResult> CreateCustomer(UserArg userArg);
		SiteCustomer GetCustomerById(string id);
		bool UpdateSiteCustomer(string id, string stripeUserToken);
	}
}