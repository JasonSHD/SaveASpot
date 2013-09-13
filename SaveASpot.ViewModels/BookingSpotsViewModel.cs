using System.Collections.Generic;
using SaveASpot.Core;

namespace SaveASpot.ViewModels
{
	public sealed class BookingSpotsViewModel
	{
		public IEnumerable<IElementIdentity> BookedSpots { get; set; }
	}
}