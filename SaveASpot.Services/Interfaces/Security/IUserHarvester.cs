using SaveASpot.Repositories.Models.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserHarvester
	{
		UserViewModel Convert(User user);
		UserViewModel Anonyms();
		UserViewModel NotExists();
	}
}