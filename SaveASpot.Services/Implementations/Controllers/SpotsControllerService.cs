using System.Linq;
using MongoDB.Bson;
using SaveASpot.Core;
using SaveASpot.Core.Security;
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
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ITextParserEngine _textParserEngine;
		private readonly ITextService _textService;
		private readonly ICurrentCustomer _currentCustomer;

		public SpotsControllerService(ISpotRepository spotRepository, ISpotQueryable spotQueryable, IParcelQueryable parcelQueryable, ITextParserEngine textParserEngine, ITextService textService, ICurrentCustomer currentCustomer)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
			_parcelQueryable = parcelQueryable;
			_textParserEngine = textParserEngine;
			_textService = textService;
			_currentCustomer = currentCustomer;
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

		public SpotsViewModel ByPhase(string identity)
		{
			var parcelsForPhase = _parcelQueryable.Find(_parcelQueryable.ByPhase(identity));

			return new SpotsViewModel
							 {
								 Spots = _spotQueryable.Find(_spotQueryable.ByParcels(parcelsForPhase.Select(e => e.Identity))).Select(ToSpotViewModel).ToList(),
								 Selector = new SelectorViewModel()
							 };
		}

		public IMethodResult<string> BookingSpot(string identity)
		{
			var spots = _spotQueryable.Find(_spotQueryable.ByIdentity(identity)).ToList();

			if (spots.Count > 0)
			{
				var spot = spots.First();
				ObjectId customerId;
				if (ObjectId.TryParse(_currentCustomer.Customer.Identity, out customerId))
				{
					spot.CustomerId = customerId;
					var updateResult = _spotRepository.Update(spot);

					return new MethodResult<string>(updateResult, string.Empty);
				}
			}

			return new MethodResult<string>(false, _textService.ResolveTest("SpotNotFound"));
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_spotRepository.Remove(identity), string.Empty);
		}

		private SpotViewModel ToSpotViewModel(Spot spot)
		{
			return new SpotViewModel
			{
				Identity = spot.Identity,
				Area = spot.SpotArea,
				Points = spot.SpotShape.Select(e => new ViewModels.PhasesAndParcels.Point { Latitude = e.Latitude, Longitude = e.Longitude }),
				CustomerId = spot.CustomerId == ObjectId.Empty ? string.Empty : spot.CustomerIdentity
			};
		}
	}
}