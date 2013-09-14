using System.Linq;
using SaveASpot.Core.Cart;

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
				isAnonym = source.IsAnonym(),
				isAdmin = source.IsAdmin(),
				isCustomer = source.IsCustomer()
			};
		}

		public static object AsCustomerJson(this Customer source)
		{
			return new { user = source.User.AsUserJson(), isPaymentInfoAdded = source.IsPaymentInfoAdded };
		}

		public static object AsCartJson(this Cart.Cart source)
		{
			return new { elements = source.SpotElements.Select(e => e.AsSpotElementJson()).ToArray(), price = source.Price };
		}

		public static object AsSpotElementJson(this SpotElement source)
		{
			return new { identity = source.Identity.ToString(), phase = source.PhaseElement.AsPhaseElementJson() };
		}

		public static object AsPhaseElementJson(this PhaseElement source)
		{
			return new { spotPrice = source.SpotPrice, identity = source.Identity.ToString(), name = source.Name };
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