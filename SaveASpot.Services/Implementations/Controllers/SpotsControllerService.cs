using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ITextParserEngine _textParserEngine;

		public SpotsControllerService(ISpotRepository spotRepository, ISpotQueryable spotQueryable, ITextParserEngine textParserEngine)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
			_textParserEngine = textParserEngine;
		}

		public SpotsViewModel GetSpots(SelectorViewModel selectorViewModel)
		{
			var filter = _spotQueryable.All();

			_textParserEngine.
				BuildDecimal<Spot>(e => e.SpotArea, val => _spotQueryable.And(filter, _spotQueryable.ByArea(val))).
				BuildDecimal<Spot>(e => e.SpotArea, val => _spotQueryable.And(filter, _spotQueryable.ByArea(val))).

				Build(selectorViewModel.Search);

			return new SpotsViewModel { Spots = _spotQueryable.Find(filter).Select(e => new SpotViewModel { Identity = e.Identity, Area = e.SpotArea }).ToList(), Selector = selectorViewModel };
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_spotRepository.Remove(identity), string.Empty);
		}
	}
}