using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class PhasesControllerService : IPhasesControllerService
	{
		private readonly IPhaseRepository _phaseRepository;
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public PhasesControllerService(IPhaseRepository phaseRepository, IPhaseQueryable phaseQueryable, IElementIdentityConverter elementIdentityConverter)
		{
			_phaseRepository = phaseRepository;
			_phaseQueryable = phaseQueryable;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public PhasesViewModel GetPhases(SelectorViewModel selectorViewModel)
		{
			return new PhasesViewModel
							 {
								 Phases = _phaseQueryable.Find(_phaseQueryable.All()).Select(e => new PhaseViewModel { Identity = _elementIdentityConverter.ToIdentity(e.Id), Name = e.PhaseName }),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult RemovePhases(string identity)
		{
			return new MessageMethodResult(_phaseRepository.RemovePhase(identity), string.Empty);
		}
	}
}