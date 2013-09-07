using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
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
		private readonly ICurrentCart _currentCart;

		public MapControllerService(ICurrentUser currentUser,
			ISponsorQueryable sponsorQueryable,
			IPhaseQueryable phaseQueryable,
			ITypeConverter<Phase, PhaseViewModel> typeConverter,
			IConfigurationManager configurationManager,
			IConverter<Sponsor, SponsorViewModel> converter,
			ICurrentCart currentCart)
		{
			_currentUser = currentUser;
			_sponsorQueryable = sponsorQueryable;
			_phaseQueryable = phaseQueryable;
			_typeConverter = typeConverter;
			_configurationManager = configurationManager;
			_converter = converter;
			_currentCart = currentCart;
		}

		public MapViewModel GetMapViewModel()
		{
			var viewModel = new MapViewModel
							 {
								 ShowCustomerBookingPanel = _currentUser.User.IsCustomer() || _currentUser.User.IsAnonym(),
								 Phases = _phaseQueryable.Filter(e => e.All()).Find().Select(_typeConverter.Convert).ToList(),
								 GoogleMapKey = _configurationManager.GetSettings("GoogleMapKey")

							 };

			if (viewModel.ShowCustomerBookingPanel)
			{
				viewModel.Sponsors = Enumerable.Empty<SponsorViewModel>();
				viewModel.Cart = _currentCart.Cart;
			}
			else
			{
				viewModel.Sponsors = _sponsorQueryable.Filter(e => e.All()).Find().Select(_converter.Convert);
			}

			return viewModel;
		}
	}
}