using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class SpotViewModel
	{
		public IElementIdentity Identity { get; set; }
		public decimal Area { get; set; }
		public bool IsAvailable { get { return CustomerId.IsNull(); } }
		public IElementIdentity CustomerId { get; set; }
		public IEnumerable<Point> Points { get; set; }

		public object ToJson()
		{
			return new { identity = Identity, area = Area, points = Points.Select(e => e.ToJson()), isAvailable = IsAvailable };
		}
	}
}