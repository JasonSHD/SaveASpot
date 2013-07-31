using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public interface ISponsorsService
	{
		IMethodResult<CreateSponsorResult> AddSponsor(SponsorArg sponsorArg);
		IEnumerable<Sponsor> GetAllSponsors();
	}
}