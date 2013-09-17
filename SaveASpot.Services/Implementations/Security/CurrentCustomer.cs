using System.Linq;
using System.Security.Authentication;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CurrentCustomer : ICurrentCustomer
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICustomerQueryable _customerQueryable;
		private readonly ICustomerFactory _customerFactory;

		public CurrentCustomer(ICurrentUser currentUser,
			ICustomerQueryable customerQueryable,
			ICustomerFactory customerFactory)
		{
			_currentUser = currentUser;
			_customerQueryable = customerQueryable;
			_customerFactory = customerFactory;
		}

		public Customer Customer
		{
			get
			{
				var user = _currentUser.User;
				var customers = _customerQueryable.Filter(e => e.FilterByUserId(user.Identity)).ToList();

				if (customers.Count < 1)
				{
					throw new AuthenticationException();
				}

				var customer = customers.First();

				return _customerFactory.Convert(user, customer);
			}
		}
	}
}