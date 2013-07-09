using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces
{
	public interface ICurrentUser
	{
		UserViewModel User { get; }
	}
}
