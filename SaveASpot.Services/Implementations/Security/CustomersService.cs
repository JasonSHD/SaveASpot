using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class CustomersService : ICustomerService
	{
		private readonly IUserService _userService;
		private readonly ICustomerRepository _customerRepository;
		private readonly IRoleFactory _roleFactory;

		public CustomersService(IUserService userService, ICustomerRepository customerRepository, IRoleFactory roleFactory)
		{
			_userService = userService;
			_customerRepository = customerRepository;
			_roleFactory = roleFactory;
		}

		public IMethodResult<CreateCustomerResult> CreateCustomer(UserArg userArg)
		{
			var result = _userService.CreateUser(userArg, new[] { _roleFactory.Convert(typeof(CustomerRole)) });
			var createCustomerResult = _customerRepository.CreateCustomer(result.Status.UserId);

			return new MethodResult<CreateCustomerResult>(createCustomerResult, new CreateCustomerResult { });
		}

		public SiteCustomer GetCustomerByUserId(string userId)
		{
			return _customerRepository.GetCustomerByUserId(userId);
		}

		public SiteCustomer GetCustomerById(string id)
		{
			return _customerRepository.GetCustomerById(id);
		}

		public bool UpdateSiteCustomer(string id, string stripeUserToken)
		{
			return _customerRepository.UpdateSiteCustomer(id, stripeUserToken);
		}
	}
}