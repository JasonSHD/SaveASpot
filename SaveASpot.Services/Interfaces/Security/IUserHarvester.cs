using SaveASpot.Repositories.Models.Security;
using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Interfaces.Security
{
	public interface IUserHarvester
	{
		User Convert(UserEntity userEntity);
		User NotExists();
	}
}