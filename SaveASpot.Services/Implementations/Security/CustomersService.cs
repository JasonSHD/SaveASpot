using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CustomersService : ICustomerService
	{
		private readonly IUserService _userService;
		private readonly ICustomerRepository _customerRepository;
		private readonly IRoleFactory _roleFactory;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CustomersService(IUserService userService, ICustomerRepository customerRepository, IRoleFactory roleFactory, IElementIdentityConverter elementIdentityConverter)
		{
			_userService = userService;
			_customerRepository = customerRepository;
			_roleFactory = roleFactory;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IMethodResult<CreateCustomerResult> CreateCustomer(UserArg userArg)
		{
			var result = _userService.CreateUser(userArg, new[] { _roleFactory.Convert(typeof(CustomerRole)) });
			if (result.IsSuccess)
			{
				var createCustomerResult = _customerRepository.CreateCustomer(result.Status.UserId);

				return new MethodResult<CreateCustomerResult>(true,
																											new CreateCustomerResult
																												{
																													CustomerId = createCustomerResult,
																													MessageKey = string.Empty,
																													UserId =
																														_elementIdentityConverter.ToIdentity(
																															result.Status.UserId)
																												});
			}

			return new MethodResult<CreateCustomerResult>(result.IsSuccess, new CreateCustomerResult { CustomerId = new NullElementIdentity(), MessageKey = result.Status.MessageKey, UserId = result.Status.UserId });
		}

		public bool UpdateSiteCustomer(string id, string stripeUserToken)
		{
			return _customerRepository.UpdateSiteCustomer(id, stripeUserToken);
		}
	}
}