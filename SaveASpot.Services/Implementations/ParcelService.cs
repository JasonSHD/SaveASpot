using System.Collections.Generic;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public class ParcelService:IParcelService
	{
		private readonly IParcelRepository _parcelRepository;

		public ParcelService(IParcelRepository parcelRepository)
		{
			_parcelRepository = parcelRepository;
		}

		public IEnumerable<Parcel> GetAllParcelsByPhaseId(string phaseId)
		{
			return _parcelRepository.GetAllParcelsByPhaseId(phaseId);
		}
	}
}
