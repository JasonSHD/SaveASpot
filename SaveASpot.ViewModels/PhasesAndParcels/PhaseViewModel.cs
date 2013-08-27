using System.ComponentModel.DataAnnotations;
using SaveASpot.Core;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class PhaseViewModel
	{
		public IElementIdentity Identity { get; set; }
		public string Name { get; set; }

		[Required(ErrorMessage = "Spot price is required")]
		//[Range(typeof(double), "0,50", "21474835",ErrorMessage = "Price must be a number between {0} and {1}.")]
		public string SpotPrice { get; set; }

		public object ToJson()
		{
			return new { name = Name, identity = Identity.ToString(), spotPrice=SpotPrice };
		}
	}
}
