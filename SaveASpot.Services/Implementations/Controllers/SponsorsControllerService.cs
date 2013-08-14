﻿using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SponsorsControllerService : ISponsorsControllerService
	{
		private readonly ISponsorsService _sponsorsService;

		public SponsorsControllerService(ISponsorsService sponsorsService)
		{
			_sponsorsService = sponsorsService;
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
																							new SponsorResult(new SponsorViewModel { Identity = createSponsorResult.Status.SponsorId, CompanyName = createSponsorViewModel.CompanyName, Sentence = createSponsorViewModel.Sentence, Logo = createSponsorViewModel.Logo, Url = createSponsorViewModel.Url}, "Sponsor created!"));
		}

		public IMethodResult<MessageResult> EditSponsor(string identity, SponsorViewModel sponsorViewModel)
		{
			var updateSponsorResult = _sponsorsService.EditSponsor(identity, new SponsorArg
				                             {
					                             CompanyName = sponsorViewModel.CompanyName,
					                             Logo = sponsorViewModel.Logo,
					                             Sentence = sponsorViewModel.Sentence,
					                             Url = sponsorViewModel.Url
				                             });
			return new MethodResult<MessageResult>(updateSponsorResult.IsSuccess, new MessageResult("Sponsor created!"));
		}

		public IEnumerable<SponsorViewModel> GetSponsors()
		{
			return _sponsorsService.GetAllSponsors().Select(e => new SponsorViewModel { Identity = e.Identity, CompanyName = e.CompanyName, Sentence = e.Sentence, Logo =  e.Logo, Url =  e.Url });
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_sponsorsService.Remove(identity), string.Empty);
		}
	}
}
