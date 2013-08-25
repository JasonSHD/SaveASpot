using SaveASpot.Core;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class PhaseViewModel
	{
		public IElementIdentity Identity { get; set; }
		public string Name { get; set; }
		public uint SpotPrice { get; set; }

		public object ToJson()
		{
			return new { name = Name, identity = Identity.ToString(), spotPrice=SpotPrice };
		}
	}
}
