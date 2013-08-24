using System.Collections.Generic;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public class SpotService:ISpotService
	{
		private readonly ISpotRepository _spotRepository;

		public SpotService(ISpotRepository spotRepository)
		{
			_spotRepository = spotRepository;
		}

		public IEnumerable<Spot> GetSpotsByParcelId(string parcelId)
		{
			return _spotRepository.GetSpotsByParcelId(parcelId);
		}
	}
}
