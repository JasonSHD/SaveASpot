using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICustomersControllerService
	{
		IMethodResult<CustomerResult> AddCustomer(CreateCustomerViewModel createCustomerViewModel);
		IEnumerable<CustomerViewModel> GetCustomers();
	}
}