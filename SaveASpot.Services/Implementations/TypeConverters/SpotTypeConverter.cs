using SaveASpot.Core;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.TypeConverters
{
	public sealed class SpotTypeConverter : ITypeConverter<Spot, SpotViewModel>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SpotTypeConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public SpotViewModel Convert(Spot source)
		{
			return new SpotViewModel
							 {
								 Area = source.SpotArea,
								 CustomerId = _elementIdentityConverter.ToIdentity(source.CustomerId),
								 Identity = _elementIdentityConverter.ToIdentity(source.Id),
								 Points = source.SpotShape,
								 Price = 0,
								 SponsorIdentity = _elementIdentityConverter.ToIdentity(source.SponsorId)
							 };
		}
	}
}
