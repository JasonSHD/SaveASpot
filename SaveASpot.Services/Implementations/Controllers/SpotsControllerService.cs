using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SpotsControllerService : ISpotsControllerService
	{
		private static readonly IList<SpotViewModel> SpotsViewModels = new List<SpotViewModel>();

		public SpotsViewModel GetSpots(SelectorViewModel selectorViewModel)
		{
			if (SpotsViewModels.Count == 0)
			{
				for (var index = 0; index < 10; index++)
				{
					SpotsViewModels.Add(new SpotViewModel { Name = "spot name #" + index, Identity = "spot_identity_" + index });
				}
			}

			IEnumerable<SpotViewModel> spots = SpotsViewModels;
			if (!string.IsNullOrWhiteSpace(selectorViewModel.Search))
			{
				spots = spots.Where(e => e.Name.Contains(selectorViewModel.Search));
			}
			return new SpotsViewModel { Spots = spots, Selector = selectorViewModel };
		}

		public IMethodResult Remove(string identity)
		{
			SpotsViewModels.Remove(SpotsViewModels.First(e => e.Identity == identity));

			return new MessageMethodResult(true, string.Empty);
		}
	}
}