using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Geocoding;
using SaveASpot.Services.Interfaces.PhasesAndParcels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private readonly ISpotsService _spotsService;
		private readonly ISpotQueryable _spotQueryable;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ITextParserEngine _textParserEngine;
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ISquareElementsCalculator _squareElementsCalculator;

		public SpotsControllerService(ISpotsService spotsService,
			ISpotQueryable spotQueryable,
			IParcelQueryable parcelQueryable,
			ITextParserEngine textParserEngine,
			IElementIdentityConverter elementIdentityConverter,
			ISquareElementsCalculator squareElementsCalculator)
		{
			_spotsService = spotsService;
			_spotQueryable = spotQueryable;
			_parcelQueryable = parcelQueryable;
			_textParserEngine = textParserEngine;
			_elementIdentityConverter = elementIdentityConverter;
			_squareElementsCalculator = squareElementsCalculator;
		}

		public SpotsViewModel GetSpots(SelectorViewModel selectorViewModel)
		{
			var filter = _spotQueryable.All();

			_textParserEngine.
				BuildDecimal<Spot>(e => e.SpotArea, val => _spotQueryable.And(filter, _spotQueryable.ByArea(val))).
				BuildDecimal<Spot>(e => e.SpotArea, val => _spotQueryable.And(filter, _spotQueryable.ByArea(val))).

				Build(selectorViewModel.Search);

			return new SpotsViewModel
							 {
								 Spots = _spotQueryable.Find(filter).Select(ToSpotViewModel).ToList(),
								 Selector = selectorViewModel
							 };
		}

		public SpotsViewModel ByPhase(IElementIdentity identity)
		{
			var parcelsForPhase = _parcelQueryable.Find(_parcelQueryable.ByPhase(identity));

			return new SpotsViewModel
							 {
								 Spots = _spotQueryable.Filter(s => s.ByParcels(parcelsForPhase.Select(e => _elementIdentityConverter.ToIdentity(e.Id)))).Find().Select(ToSpotViewModel).ToList(),
								 Selector = new SelectorViewModel()
							 };
		}

		public IMethodResult Remove(IElementIdentity identity)
		{
			return new MessageMethodResult(_spotsService.RemoveSpots(new[] { identity }), string.Empty);
		}

		public SquareElementsResult ForSquare(IElementIdentity phaseIdentity, Point topRight, Point bottomLeft)
		{
			return _squareElementsCalculator.FindElementsForSquare(phaseIdentity, bottomLeft, topRight);
		}

		private SpotViewModel ToSpotViewModel(Spot spot)
		{
			return new SpotViewModel
			{
				Identity = _elementIdentityConverter.ToIdentity(spot.Id),
				Area = spot.SpotArea,
				Points = spot.SpotShape.Select(e => new Point { Latitude = e.Latitude, Longitude = e.Longitude }),
				CustomerId = _elementIdentityConverter.ToIdentity(spot.CustomerId),
				SponsorIdentity = _elementIdentityConverter.ToIdentity(spot.SponsorId)
			};
		}
	}
}