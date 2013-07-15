using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserHarvester
	{
		User Convert(SiteUser siteUser);
		User NotExists();
	}
}