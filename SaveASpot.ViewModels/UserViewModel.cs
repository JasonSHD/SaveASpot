namespace SaveASpot.ViewModels
{
	public sealed class UserViewModel
	{
		private readonly string _username;
		private readonly string _email;
		public string Username { get { return _username; } }
		public string Email { get { return _email; } }

		public UserViewModel(string username, string email)
		{
			_username = username;
			_email = email;
		}

		public bool IsExists()
		{
			return !string.IsNullOrWhiteSpace(Username);
		}
	}
}