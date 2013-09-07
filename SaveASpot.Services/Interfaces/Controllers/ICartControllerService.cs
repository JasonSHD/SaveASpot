using SaveASpot.Core;
using SaveASpot.ViewModels.Carts;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface ICartControllerService
	{
		ChangeCartResultViewModel AddSpotToCart(IElementIdentity spotIdentity);
		ChangeCartResultViewModel RemoveSpotFromCart(IElementIdentity spotIdentity);
	}
}