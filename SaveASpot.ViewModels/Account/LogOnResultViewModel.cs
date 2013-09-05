using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.Account
{
	public sealed class LogOnResultViewModel
	{
		private readonly bool _isSuccess;
		private readonly string _message;
		private readonly User _user;

		public LogOnResultViewModel(bool isSuccess, string message, User user)
		{
			_isSuccess = isSuccess;
			_message = message;
			_user = user;
		}

		public bool IsSuccess { get { return _isSuccess; } }
		public string Message { get { return _message; } }
		public User User { get { return _user; } }

		public object AsJsonResult()
		{
			return new { status = IsSuccess, message = Message, user = User.AsUserJson() };
		}
	}
}