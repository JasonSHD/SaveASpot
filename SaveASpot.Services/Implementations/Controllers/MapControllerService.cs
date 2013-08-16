using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class MapControllerService : IMapControllerService
	{
		private readonly ICurrentUser _currentUser;
		private readonly IRoleFactory _roleFactory;
		private readonly ISponsorQueryable _sponsorQueryable;
		private readonly IConverter<Sponsor, SponsorViewModel> _converter;

		public MapControllerService(ICurrentUser currentUser, IRoleFactory roleFactory, ISponsorQueryable sponsorQueryable, IConverter<Sponsor, SponsorViewModel> converter)
		{
			_currentUser = currentUser;
			_roleFactory = roleFactory;
			_sponsorQueryable = sponsorQueryable;
			_converter = converter;
		}

		public MapViewModel GetMapViewModel()
		{
			var customerRole = _roleFactory.Convert(typeof(CustomerRole));

			var viewModel = new MapViewModel
							 {
								 IsCustomer = _currentUser.User.IsCustomer(customerRole),
							 };

			if (viewModel.IsCustomer)
			{
				viewModel.Sponsors = Enumerable.Empty<SponsorViewModel>();
			}
			else
			{
				viewModel.Sponsors = _sponsorQueryable.Filter(e => e.All()).Find().Select(_converter.Convert);
			}

			return viewModel;
		}
	}
}