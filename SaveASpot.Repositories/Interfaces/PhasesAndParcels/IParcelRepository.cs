using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelRepository
	{
		Parcel AddParcel(Parcel parcel);
	}
}
