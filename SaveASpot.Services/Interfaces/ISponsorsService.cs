using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISponsorsService
	{
		IMethodResult<MessageResult> AddSponsor(SponsorArg sponsorArg);
	}
}