using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class SpotViewModel
	{
		public string Identity { get; set; }
		public decimal Area { get; set; }
		public bool IsAvailable { get { return string.IsNullOrWhiteSpace(CustomerId); } }
		public string CustomerId { get; set; }
		public IEnumerable<Point> Points { get; set; }

		public object ToJson()
		{
			return new { identity = Identity, area = Area, points = Points.Select(e => e.ToJson()), isAvailable = IsAvailable };
		}
	}
}