using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserFactory
	{
		User Convert(SiteUser siteUser);
		User NotExists();
	}
}