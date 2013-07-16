using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public sealed class CustomerResult
	{
		private readonly CustomerViewModel _customerViewModel;
		private readonly string _message;
		public CustomerViewModel CustomerViewModel { get { return _customerViewModel; } }
		public string Message { get { return _message; } }

		public CustomerResult(CustomerViewModel customerViewModel, string message)
		{
			_customerViewModel = customerViewModel;
			_message = message;
		}
	}
}