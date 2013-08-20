using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.Sponsors
{
	public interface ISponsorQueryable : IElementQueryable<Sponsor, ISponsorFilter>
	{
		ISponsorFilter All();
		ISponsorFilter ByName(string name);
		ISponsorFilter ByIdentity(IElementIdentity elementIdentity);
		ISponsorFilter ByUrl(string url);
	}
}
