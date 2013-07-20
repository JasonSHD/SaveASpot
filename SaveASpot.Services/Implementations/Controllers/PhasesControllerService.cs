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

		public PhasesControllerService(IPhaseRepository phaseRepository, IPhaseQueryable phaseQueryable)
		{
			_phaseRepository = phaseRepository;
			_phaseQueryable = phaseQueryable;
		}

		public PhasesViewModel GetPhases(SelectorViewModel selectorViewModel)
		{
			return new PhasesViewModel
							 {
								 Phases = _phaseQueryable.Find(_phaseQueryable.All()).Select(e => new PhaseViewModel { Identity = e.Identity, Name = e.PhaseName }),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult RemovePhases(string identity)
		{
			return new MessageMethodResult(_phaseRepository.RemovePhase(identity), string.Empty);
		}
	}
}