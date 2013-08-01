using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public sealed class SponsorResult
	{
		private readonly SponsorViewModel _sponsorViewModel;
		private readonly string _message;
		public SponsorViewModel SponsorViewModel { get { return _sponsorViewModel; } }
		public string Message { get { return _message; } }

		public SponsorResult(SponsorViewModel sponsorViewModel, string message)
		{
			_sponsorViewModel = sponsorViewModel;
			_message = message;
		}
	}
}
