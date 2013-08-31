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
			return new { name = source.Name, email = source.Email, isAnonym = source.Roles.Any(e => e == new AnonymRole()) };
		}

		public static object AsCustomerJson(this Customer source)
		{
			return new { user = source.User.AsUserJson(), cart = source.Cart.AsCartJson() };
		}

		public static object AsCartJson(this Cart source)
		{
			return new { elements = source.ElementIdentities.Select(e => e.ToString()).ToArray() };
		}

		public static bool IsCustomer(this User source, Role customerRole)
		{
			return source.Roles.Any(e => e == customerRole);
		}

		public static bool IsAnonym(this User source, Role anonymRole)
		{
			return source.Roles.Any(e => e == anonymRole);
		}
	}
}