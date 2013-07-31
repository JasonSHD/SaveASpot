using System.ComponentModel.DataAnnotations;

namespace SaveASpot.ViewModels
{
	public sealed class CreateSponsorViewModel
	{
		[Required]
		[Display(Name = Constants.Display.SponsorCompanyName)]
		public string CompanyName { get; set; }

		[Required]
		[Display(Name = Constants.Display.SponsorSentence)]
		public string Sentence { get; set; }

		[Required]
		[Display(Name = Constants.Display.SponsorUrl)]
		public string Url { get; set; }

		[Required]
		[Display(Name = Constants.Display.SponsorLogo)]
		public string Logo { get; set; }
	}
}
