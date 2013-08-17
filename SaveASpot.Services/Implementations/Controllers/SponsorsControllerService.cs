using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SponsorsControllerService : ISponsorsControllerService
	{
		private readonly ISponsorsService _sponsorsService;
		private readonly IConverter<Sponsor, SponsorViewModel> _converter;
		private readonly ISponsorQueryable _sponsorQueryable;

		public SponsorsControllerService(ISponsorsService sponsorsService, IConverter<Sponsor, SponsorViewModel> converter, ISponsorQueryable sponsorQueryable)
		{
			_sponsorsService = sponsorsService;
			_converter = converter;
			_sponsorQueryable = sponsorQueryable;
		}

		public IMethodResult<SponsorResult> AddSponsor(CreateSponsorViewModel createSponsorViewModel)
		{
			var createSponsorResult = _sponsorsService.AddSponsor(
				new SponsorArg
				{
					CompanyName = createSponsorViewModel.CompanyName,
					Logo = createSponsorViewModel.Logo,
					Sentence = createSponsorViewModel.Sentence,
					Url = createSponsorViewModel.Url
				});

			return new MethodResult<SponsorResult>(createSponsorResult.IsSuccess,
																							new SponsorResult(_converter.Convert(createSponsorResult.Status.Sponsor), "Sponsor created!"));
		}

		public IEnumerable<SponsorViewModel> GetSponsors()
		{
			return _sponsorsService.GetAllSponsors().Select(_converter.Convert);
		}

		public SponsorViewModel SponsorDetails(IElementIdentity sponsorIdentity)
		{
			return _converter.Convert(_sponsorQueryable.Filter(e => e.ByIdentity(sponsorIdentity)).First());
		}
	}
}
