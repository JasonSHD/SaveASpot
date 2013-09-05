using SaveASpot.Core;
using SaveASpot.Core.Security;

namespace SaveASpot.Services.Interfaces
{
	public interface ICartService
	{
		void AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		void RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity);
		Cart CreateCart(IElementIdentity elementIdentity);
	}
}