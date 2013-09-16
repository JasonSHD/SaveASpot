using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using Stripe;

namespace SaveASpot.Services.Implementations.Controllers
{
	public class CardInfoControllerService : ICardInfoControllerService
	{
		private readonly ICustomerService _customerService;
		private readonly ICurrentCustomer _currentCustomer;

		public CardInfoControllerService(ICustomerService customerService, ICurrentCustomer currentCustomer)
		{
			_customerService = customerService;
			_currentCustomer = currentCustomer;
		}

		public IMethodResult CreatePaymentInformation(string token)
		{
			var customer = new StripeCustomerCreateOptions { TokenId = token };

			var stripeCustomerService = new StripeCustomerService();
			var stripeCustomer = stripeCustomerService.Create(customer);

			var result = _customerService.UpdateSiteCustomer(_currentCustomer.Customer.Identity.ToString(), stripeCustomer.Id);
			return new MessageMethodResult(result, string.Empty);
		}
	}
}
