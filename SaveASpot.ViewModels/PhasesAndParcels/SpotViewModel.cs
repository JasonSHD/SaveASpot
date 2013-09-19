using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class SpotViewModel
	{
		public IElementIdentity Identity { get; set; }
		public decimal? Area { get; set; }
		public decimal Price { get; set; }
		public bool IsAvailable { get { return CustomerId.IsNull() && SponsorIdentity.IsNull(); } }
		public IElementIdentity CustomerId { get; set; }
		public IElementIdentity SponsorIdentity { get; set; }
		public IEnumerable<Point> Points { get; set; }

		public object ToJson()
		{
			return new { identity = Identity.ToString(), area = Area, points = Points.Select(e => e.ToJson()), isAvailable = IsAvailable, sponsorId = SponsorIdentity.ToString() };
		}
	}
}