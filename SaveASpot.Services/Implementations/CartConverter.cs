using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Cart;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CartConverter : ITypeConverter<Repositories.Models.Security.Cart, Cart>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;
		private readonly ISpotPhaseContainerQueryable _spotPhaseContainerQueryable;
		private readonly ITypeConverter<SpotPhaseContainer, SpotElement> _spotTypeConverter;
		private readonly ICartAmountCalculator _cartAmountCalculator;

		public CartConverter(IElementIdentityConverter elementIdentityConverter,
			ISpotPhaseContainerQueryable spotPhaseContainerQueryable,
			ITypeConverter<SpotPhaseContainer, SpotElement> spotTypeConverter,
			ICartAmountCalculator cartAmountCalculator)
		{
			_elementIdentityConverter = elementIdentityConverter;
			_spotPhaseContainerQueryable = spotPhaseContainerQueryable;
			_spotTypeConverter = spotTypeConverter;
			_cartAmountCalculator = cartAmountCalculator;
		}

		public Cart Convert(Repositories.Models.Security.Cart source)
		{
			var spotDetails = _spotPhaseContainerQueryable.Find(source.SpotIdCollection.Select(e => _elementIdentityConverter.ToIdentity(e)));
			var elements = spotDetails.Select(e => _spotTypeConverter.Convert(e)).ToList();

			return new Cart(_elementIdentityConverter.ToIdentity(source.Id), elements, _cartAmountCalculator.CalculateAmount(elements));
		}
	}
}