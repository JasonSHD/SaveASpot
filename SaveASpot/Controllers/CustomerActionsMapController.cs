﻿using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	public sealed class CustomerActionsMapController : BaseController
	{
		private readonly ICustomerActionsMapControllerService _customerActionsMapControllerService;

		public CustomerActionsMapController(ICustomerActionsMapControllerService customerActionsMapControllerService)
		{
			_customerActionsMapControllerService = customerActionsMapControllerService;
		}

		[HttpPost]
		public JsonResult BookingSpots(IElementIdentity[] identities)
		{
			_customerActionsMapControllerService.BookingSpots(identities);

			return Json(new { });
		}
	}
}