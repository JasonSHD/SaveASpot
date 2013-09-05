using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Carts
{
	public interface ICartRepository
	{
		void AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		void RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		Cart CreateCart(IElementIdentity elementIdentity);
	}
}