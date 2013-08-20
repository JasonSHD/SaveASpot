using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISponsorsControllerService
	{
		IMethodResult<SponsorResult> AddSponsor(CreateSponsorViewModel createSponsorViewModel);
		IMethodResult<MessageResult> EditSponsor(string identity, SponsorViewModel sponsorViewModel);
		IEnumerable<SponsorViewModel> GetSponsors();
		IMethodResult Remove(string identity);
		SponsorViewModel SponsorDetails(IElementIdentity sponsorIdentity);
	}
}
