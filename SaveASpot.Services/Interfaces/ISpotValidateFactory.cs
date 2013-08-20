using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotValidateFactory
	{
		IValidator Available();
		IValidator AvailableForRemove();
	}
}