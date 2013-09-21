using System.ComponentModel.DataAnnotations;
using SaveASpot.Core;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class PhaseViewModel
	{
		public IElementIdentity Identity { get; set; }
		[Required]
		public string Name { get; set; }

		[Required(ErrorMessage = "Spot price is required")]
		[Range(0.01, 4000000, ErrorMessage = "InvalidSpotPriceValue")]
		[Display(Name = Constants.Display.SpotPrice)]
		public decimal SpotPrice { get; set; }

		public object ToJson()
		{
			return new { name = Name, identity = Identity.ToString(), spotPrice = SpotPrice };
		}
	}
}
