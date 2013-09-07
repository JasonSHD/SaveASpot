using System.Linq;
using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class CartConverter : ITypeConverter<Repositories.Models.Security.Cart, Cart>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CartConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public Cart Convert(Repositories.Models.Security.Cart source)
		{
			return new Cart(_elementIdentityConverter.ToIdentity(source.Id), source.SpotIdCollection.Select(e => _elementIdentityConverter.ToIdentity(e)).ToList());
		}
	}
}