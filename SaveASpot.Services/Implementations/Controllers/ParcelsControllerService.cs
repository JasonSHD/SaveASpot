﻿using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		private readonly IParcelRepository _parcelRepository;
		private readonly IParcelQueryable _parcelQueryable;

		public ParcelsControllerService(IParcelRepository parcelRepository, IParcelQueryable parcelQueryable)
		{
			_parcelRepository = parcelRepository;
			_parcelQueryable = parcelQueryable;
		}

		public ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel)
		{
			return new ParcelsViewModel
							 {
								 Parcels = _parcelQueryable.Find(_parcelQueryable.All()).Select(e => new ParcelViewModel { Identity = e.Identity, Name = e.ParcelName }),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult Remove(string identity)
		{
			return new MessageMethodResult(_parcelRepository.Remove(identity), string.Empty);
		}
	}
}