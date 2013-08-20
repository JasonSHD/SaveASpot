using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class ParcelViewModel
	{
		public IElementIdentity Identity { get; set; }
		public string Name { get; set; }
		public decimal? Length { get; set; }
		public IEnumerable<Point> Points { get; set; }

		public object ToJson()
		{
			return new { identity = Identity.ToString(), name = Name, length = Length, points = Points.Select(e => e.ToJson()).ToList() };
		}
	}
}