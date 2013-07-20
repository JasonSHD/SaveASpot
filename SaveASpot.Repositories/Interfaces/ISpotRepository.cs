using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces
{
	public interface ISpotRepository
	{
		Spot AddSpot(Spot spot);
	}
}
