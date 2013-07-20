using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		private static readonly IList<ParcelViewModel> ParcelsViewModels = new List<ParcelViewModel>();

		public ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel)
		{
			if (ParcelsViewModels.Count == 0)
			{
				for (var index = 0; index < 10; index++)
				{
					ParcelsViewModels.Add(new ParcelViewModel { Name = "parcel name #" + index, Identity = "parcel_identity_" + index });
				}
			}

			IEnumerable<ParcelViewModel> parcels = ParcelsViewModels;
			if (!string.IsNullOrWhiteSpace(selectorViewModel.Search))
			{
				parcels = parcels.Where(e => e.Name.Contains(selectorViewModel.Search));
			}
			return new ParcelsViewModel { Parcels = parcels, SelectorViewModel = selectorViewModel };
		}

		public IMethodResult Remove(string identity)
		{
			ParcelsViewModels.Remove(ParcelsViewModels.First(e => e.Identity == identity));

			return new MessageMethodResult(true, string.Empty);
		}
	}
}