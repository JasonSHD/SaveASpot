using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ISponsorsControllerService
	{
		IMethodResult<SponsorResult> AddSponsor(CreateSponsorViewModel createSponsorViewModel);
		IEnumerable<SponsorViewModel> GetSponsors();
	}
}
