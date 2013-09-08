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
			return new
			{
				name = source.Name,
				email = source.Email,
				isAnonym = source.Roles.Any(e => e == new AnonymRole()),
				isAdmin = source.Roles.Any(e => e == new AdministratorRole())
			};
		}

		public static object AsCustomerJson(this Customer source)
		{
			return new { user = source.User.AsUserJson() };
		}

		public static object AsCartJson(this Cart source)
		{
			return new { elements = source.ElementIdentities.Select(e => e.ToString()).ToArray() };
		}

		public static bool IsCustomer(this User source)
		{
			return source.Roles.Any(e => e == new CustomerRole());
		}

		public static bool IsAdmin(this User source)
		{
			return source.Roles.Any(e => e == new AdministratorRole());
		}

		public static bool IsAnonym(this User source)
		{
			return source.Roles.Any(e => e == new AnonymRole());
		}

		public static bool IsAnonym(this User source, Role anonymRole)
		{
			return source.Roles.Any(e => e == anonymRole);
		}
	}
}