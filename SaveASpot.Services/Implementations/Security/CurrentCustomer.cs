using System.Linq;
using System.Security.Authentication;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CurrentCustomer : ICurrentCustomer
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICustomerQueryable _customerQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CurrentCustomer(ICurrentUser currentUser, ICustomerQueryable customerQueryable, IElementIdentityConverter elementIdentityConverter)
		{
			_currentUser = currentUser;
			_customerQueryable = customerQueryable;
			_elementIdentityConverter = elementIdentityConverter;
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

				return new Customer(_elementIdentityConverter.ToIdentity(customer.Id), user, !string.IsNullOrEmpty(customer.StripeUserId));
			}
		}
	}
}