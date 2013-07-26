using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class ParcelViewModel
	{
		public string Identity { get; set; }
		public string Name { get; set; }
		public decimal Length { get; set; }
		public IEnumerable<Point> Points { get; set; }

		public object ToJson()
		{
			return new { identity = Identity, name = Name, length = Length, points = Points.Select(e => e.ToJson()).ToList() };
		}
	}
}