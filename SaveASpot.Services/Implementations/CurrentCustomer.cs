using System.Linq;
using System.Security.Authentication;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.Services.Implementations
{
	public sealed class CurrentCustomer : ICurrentCustomer
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICustomerQueryable _customerQueryable;

		public CurrentCustomer(ICurrentUser currentUser, ICustomerQueryable customerQueryable)
		{
			_currentUser = currentUser;
			_customerQueryable = customerQueryable;
		}

		public Customer Customer
		{
			get
			{
				var user = _currentUser.User;
				var customers = _customerQueryable.Find(_customerQueryable.FilterByUserId(user.Identity)).ToList();

				if (customers.Count < 1)
				{
					throw new AuthenticationException();
				}

				var customer = customers.First();

				return new Customer(customer.Identity, user);
			}
		}
	}
}