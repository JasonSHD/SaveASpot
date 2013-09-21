using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.PhasesAndParcels
{
	public sealed class PhasesService : IPhasesService
	{
		private readonly IPhaseRepository _phaseRepository;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly IParcelsService _parcelsService;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public PhasesService(IPhaseRepository phaseRepository,
			IParcelQueryable parcelQueryable,
			IParcelsService parcelsService,
			IElementIdentityConverter elementIdentityConverter)
		{
			_phaseRepository = phaseRepository;
			_parcelQueryable = parcelQueryable;
			_parcelsService = parcelsService;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public bool RemovePhase(IElementIdentity phaseIdentity)
		{
			var pacelsForPhase = _parcelQueryable.Filter(e => e.ByPhase(phaseIdentity));
			_phaseRepository.RemovePhase(phaseIdentity);
			_parcelsService.RemoveParcels(pacelsForPhase.Select(e => _elementIdentityConverter.ToIdentity(e.Id)));

			return true;
		}
	}
}