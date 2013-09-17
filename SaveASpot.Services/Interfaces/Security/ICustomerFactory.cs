using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface ICustomerFactory
	{
		Customer Convert(User user, SiteCustomer siteCustomer);
		Customer NotCustomer();
	}
}