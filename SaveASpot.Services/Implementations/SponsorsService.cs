using SaveASpot.Core;
using SaveASpot.Services.Interfaces;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Implementations
{
	public sealed class SponsorsService : ISponsorsService
	{
		private readonly ISponsorRepository _sponsorRepository;

		public SponsorsService(ISponsorRepository sponsorRepository)
		{
			_sponsorRepository = sponsorRepository;
		}

		public IMethodResult<MessageResult> AddSponsor(SponsorArg sponsorArg)
		{
			var sponsor = _sponsorRepository.AddSponsor(new Sponsor()
				                              {
					                              CompanyName = sponsorArg.CompanyName,
					                              Sentence = sponsorArg.Sentence,
					                              Logo = sponsorArg.Logo,
					                              Url = sponsorArg.Url
				                              });
			
			return string.IsNullOrEmpty(sponsor.Identity) 
				? new MessageMethodResult(false, "Error occured during sponsor creating") 
				: new MessageMethodResult(true, sponsor.Identity);
		}
	}
}
