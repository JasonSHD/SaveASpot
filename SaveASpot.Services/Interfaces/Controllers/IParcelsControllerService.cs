<<<<<<< HEAD
﻿using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;
=======
﻿using System.Collections.Generic;
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IParcelsControllerService
	{
<<<<<<< HEAD
		ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel);
		IMethodResult Remove(string identity);
=======
		IEnumerable<ParcelViewModel> GetParcels();
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
	}
}