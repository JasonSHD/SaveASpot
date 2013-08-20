using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public sealed class CreateSponsorResult
	{
		public string MessageKet { get; set; }
		public Sponsor Sponsor { get; set; }
	}
}
