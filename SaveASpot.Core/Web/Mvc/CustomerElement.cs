using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class CustomerElement
	{
		public static void SetCustomer(Customer customer, dynamic viewBag)
		{
			viewBag.CurrentCustomer = customer;
		}

		public static Customer GetCustomer(dynamic viewBag)
		{
			return viewBag.CurrentCustomer;
		}
	}
}