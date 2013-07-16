namespace SaveASpot.ViewModels
{
	public static class Constants
	{
		public const string EmailRegularExpression = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
		public const int PasswordMinLength = 6;
		public const int PasswordMaxLength = 100;

		public static class Display
		{
			public const string Username = "User name";
			public const string Email = "Email address";
			public const string Password = "Password";
			public const string ConfirmPassword = "Confirm password";
			public const string CustomerName = "Customer name";
			public const string ConfirmPasswordErrorMessage = "The password and confirmation password do not match.";
			public const string PasswordStringLengthErrorMessage = "The {0} must be at least {2} characters long.";
		}
	}
}
