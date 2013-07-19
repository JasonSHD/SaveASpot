using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels;

namespace SaveASpot.Controllers
{
	[MainMenuTab(Alias = SiteConstants.CustomersControllerAlias, Area = "", IndexOfOrder = 30, Title = "CustomersTabTitle")]
	[AdministratorAuthorize]
	public sealed class CustomersController : AdminTabController
	{
		private readonly ICustomersControllerService _customersControllerService;

		public CustomersController(ICustomersControllerService customersControllerService)
		{
			_customersControllerService = customersControllerService;
		}

		public ActionResult Index()
		{
			return TabView(_customersControllerService.GetCustomers());
		}

		[HttpGet]
		public ViewResult CreateCustomer()
		{
			return View();
		}

		[HttpPost]
		public ActionResult CreateCustomer(CreateCustomerViewModel createCustomerViewModel)
		{
			var createCustomerResult = _customersControllerService.AddCustomer(createCustomerViewModel);

			return Json(new { status = createCustomerResult.IsSuccess, message = createCustomerResult.Status.Message });
		}
	}
}