using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Interfaces
{
	public sealed class RemoveSpotArg
	{
		public Spot Spot { get; set; }
		public IElementIdentity CustomerIdentity { get; set; }
	}
}