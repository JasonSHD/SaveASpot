using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotQueryable _spotQueryable;

		public SpotsControllerService(ISpotRepository spotRepository, ISpotQueryable spotQueryable)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
		}

		public SpotsViewModel GetSpots(SelectorViewModel selectorViewModel)
		{
			return new SpotsViewModel { Spots = _spotQueryable.Find(_spotQueryable.All()).Select(e => new SpotViewModel { Identity = e.Identity, Area = e.SpotArea }).ToList(), Selector = selectorViewModel };
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_spotRepository.Remove(identity), string.Empty);
		}
	}
}