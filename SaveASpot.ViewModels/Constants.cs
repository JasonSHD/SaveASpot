namespace SaveASpot.ViewModels
{
	public static class Constants
	{
		public const string EmailRegularExpression = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
		public const int PasswordMinLength = 6;
		public const int PasswordMaxLength = 100;

		public static class Display
		{
			public const string Username = "UserNameDisplay";
			public const string Email = "EmailDisplay";
			public const string Password = "PasswordDisplay";
			public const string ConfirmPassword = "ConfirmPasswordDisplay";
			public const string CustomerName = "CustomerNameDisplay";
			public const string ConfirmPasswordErrorMessage = "The password and confirmation password do not match.";
			public const string PasswordStringLengthErrorMessage = "The {0} must be at least {2} characters long.";
			public const string RememberMe = "RememberMeDisplay";
		}
	}
}
