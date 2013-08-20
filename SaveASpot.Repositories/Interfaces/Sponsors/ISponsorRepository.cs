using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.Sponsors
{
	public interface ISponsorRepository
	{
		Sponsor AddSponsor(Sponsor sponsor);
		bool Remove(string identity);
		bool UpdateSponsor(string identity, Sponsor sponsor);
	}
}
