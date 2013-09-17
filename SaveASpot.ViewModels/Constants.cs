namespace SaveASpot.ViewModels
{
	public static partial class Constants
	{
		public const string EmailRegularExpression = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
		public const int PasswordMinLength = 6;
		public const int PasswordMaxLength = 100;
	}
}
