using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CustomersControllerService : ICustomersControllerService
	{
		private readonly IUserService _userService;
		private readonly ITextService _textService;
		private readonly IRoleFactory _roleFactory;

		public CustomersControllerService(IUserService userService, ITextService textService, IRoleFactory roleFactory)
		{
			_userService = userService;
			_textService = textService;
			_roleFactory = roleFactory;
		}

		public IMethodResult<CustomerResult> AddCustomer(CreateCustomerViewModel createCustomerViewModel)
		{
			var createUserResult = _userService.CreateUser(
				new UserArg
					{
						Email = createCustomerViewModel.Email,
						Password = createCustomerViewModel.Password,
						Username = createCustomerViewModel.UserName
					}, new[] { _roleFactory.Convert(typeof(CustomerRole)) });

			return new MethodResult<CustomerResult>(createUserResult.IsSuccess,
																							new CustomerResult(new CustomerViewModel { Email = createCustomerViewModel.Email, Username = createCustomerViewModel.UserName, Identity = createUserResult.Status.UserId }, _textService.ResolveTest(createUserResult.Status.MessageKet)));
		}

		public IEnumerable<CustomerViewModel> GetCustomers()
		{
			return
				_userService.GetByRole(_roleFactory.Convert(typeof(CustomerRole)))
										.Select(e => new CustomerViewModel { Email = e.Email, Identity = e.Identity, Username = e.Name });
		}
	}
}