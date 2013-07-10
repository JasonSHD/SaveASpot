namespace SaveASpot.ViewModels.Security
{
	public static class UserExtensions
	{
		public static bool IsExists(this User source)
		{
			return string.IsNullOrWhiteSpace(source.Name);
		}
	}
}