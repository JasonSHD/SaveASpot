using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Geocoding;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.TypeConverters
{
	public sealed class ParcelTypeConverter : ITypeConverter<Parcel, ParcelViewModel>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ParcelTypeConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public ParcelViewModel Convert(Parcel source)
		{
			return new ParcelViewModel { Name = source.ParcelName, Identity = _elementIdentityConverter.ToIdentity(source.Id), Length = source.ParcelLength, Points = source.ParcelShape.Select(e => new Point { Latitude = e.Latitude, Longitude = e.Longitude }) };
		}
	}
}