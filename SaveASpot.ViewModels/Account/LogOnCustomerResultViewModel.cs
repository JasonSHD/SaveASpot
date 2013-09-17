using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Account
{
	public sealed class LogOnCustomerResultViewModel
	{
		private readonly bool _isSuccess;
		private readonly string _message;
		private readonly Customer _customer;

		public bool IsSuccess { get { return _isSuccess; } }
		public string Message { get { return _message; } }
		public Customer Customer { get { return _customer; } }

		public LogOnCustomerResultViewModel(bool isSuccess, string message, Customer customer)
		{
			_isSuccess = isSuccess;
			_message = message;
			_customer = customer;
		}

		public object AsJsonResult()
		{
			return
				new
					{
						status = IsSuccess,
						message = Message,
						customer = Customer.AsCustomerJson(),
						fullUser = Customer.AsCustomerJson()
					};
		}
	}
}