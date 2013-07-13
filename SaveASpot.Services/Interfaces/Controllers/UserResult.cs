using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public sealed class UserResult
	{
		private readonly User _user;
		private readonly string _message;
		public User User { get { return _user; } }
		public string Message { get { return _message; } }

		public UserResult(User user, string message)
		{
			_user = user;
			_message = message;
		}
	}
}