using SaveASpot.Core;

namespace SaveASpot.ViewModels
{
	public sealed class SponsorViewModel
	{
		public IElementIdentity Identity { get; set; }
		public string CompanyName { get; set; }
		public string Sentence { get; set; }
		public string Url { get; set; }
		public string Logo { get; set; }
	}
}
