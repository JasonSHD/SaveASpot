using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Geocoding;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Geocoding;
using SaveASpot.ViewModels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class MapControllerService : IMapControllerService
	{
		private readonly ICurrentUser _currentUser;
		private readonly ISponsorQueryable _sponsorQueryable;
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly ITypeConverter<Phase, PhaseViewModel> _typeConverter;
		private readonly IConfigurationManager _configurationManager;
		private readonly IConverter<Sponsor, SponsorViewModel> _converter;
		private readonly ISquareElementsCalculator _squareElementsCalculator;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ITypeConverter<Parcel, ParcelViewModel> _parcelTypeConverter;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public MapControllerService(ICurrentUser currentUser,
			ISponsorQueryable sponsorQueryable,
			IPhaseQueryable phaseQueryable,
			ITypeConverter<Phase, PhaseViewModel> typeConverter,
			IConfigurationManager configurationManager,
			IConverter<Sponsor, SponsorViewModel> converter,
			ISquareElementsCalculator squareElementsCalculator, 
			IParcelQueryable parcelQueryable, 
			ITypeConverter<Parcel, ParcelViewModel> parcelTypeConverter, 
			IElementIdentityConverter elementIdentityConverter)
		{
			_currentUser = currentUser;
			_sponsorQueryable = sponsorQueryable;
			_phaseQueryable = phaseQueryable;
			_typeConverter = typeConverter;
			_configurationManager = configurationManager;
			_converter = converter;
			_squareElementsCalculator = squareElementsCalculator;
			_parcelQueryable = parcelQueryable;
			_parcelTypeConverter = parcelTypeConverter;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public MapViewModel GetMapViewModel()
		{
			var allPhases = _phaseQueryable.Filter(e => e.All()).ToList();

			var viewModel = new MapViewModel
							 {
								 ShowCustomerBookingPanel = _currentUser.User.IsCustomer() || _currentUser.User.IsAnonym(),
								 Phases = allPhases.Select(_typeConverter.Convert).ToList(),
								 GoogleMapKey = _configurationManager.GetSettings("GoogleMapKey"),
								 Parcels = _parcelQueryable.Filter(e => e.ByPhases(allPhases.Select(p => _elementIdentityConverter.ToIdentity(p.Id)))).Select(e => _parcelTypeConverter.Convert(e))
							 };

			if (viewModel.ShowCustomerBookingPanel)
			{
				viewModel.Sponsors = Enumerable.Empty<SponsorViewModel>();
			}
			else
			{
				viewModel.Sponsors = _sponsorQueryable.Filter(e => e.All()).Find().Select(_converter.Convert);
			}

			return viewModel;
		}

		public SquareElementsResult ForSquare(IElementIdentity phaseIdentity, Point topRight, Point bottomLeft)
		{
			return _squareElementsCalculator.FindElementsForSquare(phaseIdentity, bottomLeft, topRight);
		}
	}
}