using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces
{
	public interface IParcelRepository
	{
		Parcel AddParcel(Parcel parcel);
	}
}
