using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.PhasesAndParcels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		private readonly IParcelsService _parcelsService;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ITypeConverter<Parcel, ParcelViewModel> _parcelTypeConverter;

		public ParcelsControllerService(IParcelsService parcelsService,
			IParcelQueryable parcelQueryable, 
			ITypeConverter<Parcel, ParcelViewModel> parcelTypeConverter)
		{
			_parcelsService = parcelsService;
			_parcelQueryable = parcelQueryable;
			_parcelTypeConverter = parcelTypeConverter;
		}

		public ParcelsViewModel GetParcels(SelectorViewModel selectorViewModel)
		{
			return new ParcelsViewModel
							 {
								 Parcels = _parcelQueryable.Find(_parcelQueryable.All()).Select(parcel => _parcelTypeConverter.Convert(parcel)),
								 SelectorViewModel = selectorViewModel
							 };
		}

		public IMethodResult Remove(IElementIdentity identity)
		{
			return new MessageMethodResult(_parcelsService.RemoveParcels(new[] { identity }), string.Empty);
		}
	}
}