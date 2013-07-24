namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class ParcelViewModel
	{
		public string Identity { get; set; }
		public string Name { get; set; }

		public object ToJson()
		{
			return new { identity = Identity, name = Name };
		}
	}
}