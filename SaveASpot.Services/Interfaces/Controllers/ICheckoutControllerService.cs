﻿using SaveASpot.Core;
using SaveASpot.ViewModels.Checkout;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICheckoutControllerService
	{
		SpotsCheckoutViewModel GetSpots(IElementIdentity phaseIdentity);
	}
}