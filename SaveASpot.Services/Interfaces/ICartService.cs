using SaveASpot.Core;
using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces
{
	public interface ICartService
	{
		IMethodResult<string> AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		IMethodResult<string> RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		Cart CreateCart(IElementIdentity elementIdentity);
	}
}