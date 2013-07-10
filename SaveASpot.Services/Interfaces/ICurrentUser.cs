using SaveASpot.ViewModels.Security;

namespace SaveASpot.Services.Interfaces
{
	public interface ICurrentUser
	{
		User User { get; }
	}
}
