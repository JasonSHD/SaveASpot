using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.PhasesAndParcels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		private readonly IParcelsService _parcelsService;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ParcelsControllerService(IParcelsService parcelsService, IParcelQueryable parcelQueryable, IElementIdentityConverter elementIdentityConverter)
		{
			_parcelsService = parcelsService;
			_parcelQueryable = parcelQueryable;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel)
		{
			return new ParcelsViewModel
							 {
								 Parcels = _parcelQueryable.Find(_parcelQueryable.All()).Select(ToParcelViewModel),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult Remove(IElementIdentity identity)
		{
			return new MessageMethodResult(_parcelsService.RemoveParcels(new[] { identity }), string.Empty);
		}

		private ParcelViewModel ToParcelViewModel(Parcel parcel)
		{
			return new ParcelViewModel { Name = parcel.ParcelName, Identity = _elementIdentityConverter.ToIdentity(parcel.Id), Length = parcel.ParcelLength, Points = parcel.ParcelShape.Select(e => new Point { Latitude = e.Latitude, Longitude = e.Longitude }) };
		}
	}
}