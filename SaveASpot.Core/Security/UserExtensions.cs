using System.Linq;

namespace SaveASpot.Core.Security
{
	public static class UserExtensions
	{
		public static bool IsExists(this User source)
		{
			return !string.IsNullOrWhiteSpace(source.Name);
		}

		public static object AsUserJson(this User source)
		{
			return new { name = source.Name, email = source.Email };
		}

		public static bool IsCustomer(this User source, Role customerRole)
		{
			return source.Roles.Any(e => e == customerRole);
		}
	}
}