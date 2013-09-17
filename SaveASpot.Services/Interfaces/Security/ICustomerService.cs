using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface ICustomerService
	{
		IMethodResult<CreateCustomerResult> CreateCustomer(UserArg userArg);
		bool UpdateSiteCustomer(IElementIdentity id, string stripeUserToken);
	}
}