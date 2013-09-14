using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
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
		private readonly ISpotQueryable _spotQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SponsorsControllerService(ISponsorsService sponsorsService,
			IConverter<Sponsor, SponsorViewModel> converter,
			ISponsorQueryable sponsorQueryable,
			ISpotQueryable spotQueryable,
			IElementIdentityConverter elementIdentityConverter)
		{
			_sponsorsService = sponsorsService;
			_converter = converter;
			_sponsorQueryable = sponsorQueryable;
			_spotQueryable = spotQueryable;
			_elementIdentityConverter = elementIdentityConverter;
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
			return _sponsorsService.GetAllSponsors().Select(_converter.Convert);
		}

		public IMethodResult<SponsorViewModel> SponsorDetails(IElementIdentity spotIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(spotIdentity)).First();
			var sponsorIdentity = _elementIdentityConverter.ToIdentity(spot.SponsorId);
			var sponsors =
				_sponsorQueryable.Filter(e => e.ByIdentity(sponsorIdentity)).Select(e => _converter.Convert(e)).ToList();

			return new MethodResult<SponsorViewModel>(sponsors.Any(), sponsors.FirstOrDefault());
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_sponsorsService.Remove(identity), string.Empty);
		}
	}
}
