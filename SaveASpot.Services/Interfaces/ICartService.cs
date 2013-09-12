using SaveASpot.Core;
using SaveASpot.Core.Cart;

namespace SaveASpot.Services.Interfaces
{
	public interface ICartService
	{
		IMethodResult<string> AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		IMethodResult<string> RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		Cart CreateCart(IElementIdentity elementIdentity);
	}
}