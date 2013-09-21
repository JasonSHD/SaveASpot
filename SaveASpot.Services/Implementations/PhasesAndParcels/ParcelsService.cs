using System.Collections.Generic;
using System.IO;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.PhasesAndParcels
{
	public sealed class ParcelsService : IParcelsService
	{
		private readonly IParcelRepository _parcelRepository;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ISpotsService _spotService;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly IParcelsParceService _parcelsParceService;

		public ParcelsService(IParcelRepository parcelRepository,
		                      ISpotQueryable spotQueryable,
		                      ISpotsService spotService,
		                      IElementIdentityConverter elementIdentityConverter,
		                      IParcelsParceService parcelsParceService)
		{
			_parcelRepository = parcelRepository;
			_spotQueryable = spotQueryable;
			_spotService = spotService;
			_elementIdentityConverter = elementIdentityConverter;
			_parcelsParceService = parcelsParceService;
		}

		public bool RemoveParcels(IEnumerable<IElementIdentity> parcelIdentity)
		{
			var spotsInParcels = _spotQueryable.Filter(e => e.ByParcels(parcelIdentity));
			_parcelRepository.Remove(parcelIdentity);
			_spotService.RemoveSpots(spotsInParcels.Select(e => _elementIdentityConverter.ToIdentity((object) e.Id)));

			return true;
		}

		public IMethodResult<MessageResult> AddParcels(StreamReader input, decimal spotPrice)
		{
			return _parcelsParceService.AddParcels(input, spotPrice);
		}
	}
}