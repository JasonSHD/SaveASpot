using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CustomerFactory : ICustomerFactory
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly IUserFactory _userFactory;

		public CustomerFactory(IElementIdentityConverter elementIdentityConverter, IUserFactory userFactory)
		{
			_elementIdentityConverter = elementIdentityConverter;
			_userFactory = userFactory;
		}

		public Customer Convert(User siteUser, SiteCustomer siteCustomer)
		{
			return new Customer(_elementIdentityConverter.ToIdentity(siteCustomer.Id), siteUser, !string.IsNullOrWhiteSpace(siteCustomer.StripeUserId));
		}

		public Customer NotCustomer()
		{
			return new Customer(new NullElementIdentity(), _userFactory.AnonymUser(), false);
		}
	}
}