using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class ParcelsControllerService : IParcelsControllerService
	{
		private readonly IParcelRepository _parcelRepository;
		private readonly IParcelQueryable _parcelQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ParcelsControllerService(IParcelRepository parcelRepository, IParcelQueryable parcelQueryable, IElementIdentityConverter elementIdentityConverter)
		{
			_parcelRepository = parcelRepository;
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
			return new MessageMethodResult(_parcelRepository.Remove(identity), string.Empty);
		}

		private ParcelViewModel ToParcelViewModel(Parcel parcel)
		{
			return new ParcelViewModel { Name = parcel.ParcelName, Identity = _elementIdentityConverter.ToIdentity(parcel.Id), Length = parcel.ParcelLength, Points = parcel.ParcelShape.Select(e => new ViewModels.PhasesAndParcels.Point { Latitude = e.Latitude, Longitude = e.Longitude }) };
		}
	}
}