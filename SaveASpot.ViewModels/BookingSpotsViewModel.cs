using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels
{
	public sealed class BookingSpotsViewModel
	{
		public IEnumerable<IElementIdentity> BookedSpots { get; set; }
		public Cart Cart { get; set; }
	}
}