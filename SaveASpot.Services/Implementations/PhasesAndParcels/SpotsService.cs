using System.Collections.Generic;
using System.IO;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.PhasesAndParcels
{
	public sealed class SpotsService : ISpotsService
	{
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotsParceService _spotsParceService;

		public SpotsService(ISpotRepository spotRepository, ISpotsParceService spotsParceService)
		{
			_spotRepository = spotRepository;
			_spotsParceService = spotsParceService;
		}

		public bool RemoveSpots(IEnumerable<IElementIdentity> spotIdentityCollection)
		{
			_spotRepository.Remove(spotIdentityCollection);

			return true;
		}

		public IMethodResult<MessageResult> AddSpots(StreamReader streamReader)
		{
			return _spotsParceService.AddSpots(streamReader);
		}
	}
}