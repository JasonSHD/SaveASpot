using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface ISpotRepository
	{
		Spot AddSpot(Spot spot);
		bool Remove(string identity);
		bool Update(Spot spot);
	}
}
