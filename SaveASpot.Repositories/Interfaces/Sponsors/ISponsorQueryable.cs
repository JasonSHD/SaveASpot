using System.Collections.Generic;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.Sponsors
{
	public interface ISponsorQueryable
	{
		ISponsorFilter All();
		ISponsorFilter ByName(string name);
		ISponsorFilter ByUrl(string url);
		IEnumerable<Sponsor> Find(ISponsorFilter sponsorFilter);
	}
}
