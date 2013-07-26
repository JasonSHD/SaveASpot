namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class PhaseViewModel
	{
		public string Identity { get; set; }
		public string Name { get; set; }

		public object ToJson()
		{
			return new { name = Name, identity = Identity };
		}
	}
}
