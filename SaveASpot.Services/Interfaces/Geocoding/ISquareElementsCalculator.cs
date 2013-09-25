using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Geocoding
{
	public interface ISquareElementsCalculator
	{
		SquareElementsResult FindElementsForSquare(IElementIdentity phase, Point leftBottom, Point rightTop);
	}

	public sealed class SquareElementsResult
	{
		public Point Center { get; set; }
		public ResultStatus Status { get; set; }
		public string Message { get; set; }
		public IEnumerable<SpotViewModel> Spots { get; set; }
		public IEnumerable<ParcelViewModel> Parcels { get; set; } 

		public enum ResultStatus
		{
			//Much,
			Phase,
			NotFound,
			All,
			Part,
			Last
		}
	}
}
