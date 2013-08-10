using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CustomersControllerService : ICustomersControllerService
	{
		private readonly ICustomerService _customerService;
		private readonly ICustomerQueryable _customerQueryable;
		private readonly ITextService _textService;
		private readonly ITypeConverter<SiteCustomer, CustomerViewModel> _typeConverter;

		public CustomersControllerService(ICustomerService customerService, ICustomerQueryable customerQueryable, ITextService textService, ITypeConverter<SiteCustomer, CustomerViewModel> typeConverter)
		{
			_customerService = customerService;
			_customerQueryable = customerQueryable;
			_textService = textService;
			_typeConverter = typeConverter;
		}

		public IMethodResult<CustomerResult> AddCustomer(CreateCustomerViewModel createCustomerViewModel)
		{
			var createUserResult = _customerService.CreateCustomer(
				new UserArg
					{
						Email = createCustomerViewModel.Email,
						Password = createCustomerViewModel.Password,
						Username = createCustomerViewModel.UserName
					});

			return new MethodResult<CustomerResult>(createUserResult.IsSuccess,
																							new CustomerResult(new CustomerViewModel { Email = createCustomerViewModel.Email, Username = createCustomerViewModel.UserName, Identity = createUserResult.Status.UserId }, _textService.ResolveTest(createUserResult.Status.MessageKey)));
		}

		public IEnumerable<CustomerViewModel> GetCustomers()
		{
			return _customerQueryable.Filter(e => e.All()).Find().Select(e => _typeConverter.Convert(e));
		}
	}
}