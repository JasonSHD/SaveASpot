using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.PhasesAndParcels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class PhasesControllerService : IPhasesControllerService
	{
		private readonly IPhaseRepository _phaseRepository;
		private readonly IPhasesService _phasesService;
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly ITypeConverter<Phase, PhaseViewModel> _typeConverter;

		public PhasesControllerService(IPhaseRepository phaseRepository,
			IPhasesService phasesService,
			IPhaseQueryable phaseQueryable,
			ITypeConverter<Phase, PhaseViewModel> typeConverter)
		{
			_phaseRepository = phaseRepository;
			_phasesService = phasesService;
			_phaseQueryable = phaseQueryable;
			_typeConverter = typeConverter;
		}

		public PhasesViewModel GetPhases(SelectorViewModel selectorViewModel)
		{
			return new PhasesViewModel
							 {
								 Phases = _phaseQueryable.Find(_phaseQueryable.All()).Select(_typeConverter.Convert),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult RemovePhases(IElementIdentity identity)
		{
			return new MessageMethodResult(_phasesService.RemovePhase(identity), string.Empty);
		}

		public IMethodResult<MessageResult> EditPhase(IElementIdentity identity, PhaseViewModel phaseViewModel)
		{
			var result = _phaseRepository.UpdatePhase(identity, new Phase
			{
				PhaseName = phaseViewModel.Name,
				SpotPrice = phaseViewModel.SpotPrice
			});

			return result
							 ? new MessageMethodResult(true, string.Empty)
							 : new MessageMethodResult(false, "Error occured during phase upading.");
		}
	}
}