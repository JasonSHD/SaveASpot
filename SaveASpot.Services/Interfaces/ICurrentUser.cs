using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces
{
	public interface ICurrentUser
	{
		User User { get; }
	}
}
