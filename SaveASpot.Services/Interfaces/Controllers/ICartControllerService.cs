using SaveASpot.Core;
using SaveASpot.ViewModels.Carts;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICartControllerService
	{
		AddSpotToCartResultViewModel AddSpotToCart(IElementIdentity spotIdentity);
		void RemoveSpotFromCart(IElementIdentity spotIdentity);
	}
}