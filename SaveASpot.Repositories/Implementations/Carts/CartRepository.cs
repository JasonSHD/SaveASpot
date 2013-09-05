using System.Linq;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Carts;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Carts
{
	public sealed class CartRepository : ICartRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public CartRepository(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public void AddSpotToCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			var cart = FindFirstCart(cartIdentity);

			cart.SpotIdCollection = cart.SpotIdCollection.Union(new[] { spotIdentity.ToIdentity() }).ToArray();
			_mongoDBCollectionFactory.Collection<Cart>().Save(cart);
		}

		public void RemoveSpotFromCart(IElementIdentity cartIdentity, IElementIdentity spotIdentity)
		{
			var cart = FindFirstCart(cartIdentity);
			var spotsInCart = cart.SpotIdCollection.ToList();
			var spotId = spotIdentity.ToIdentity();
			var spotIdOnRemove = spotsInCart.FirstOrDefault(e => e == spotId);
			spotsInCart.Remove(spotIdOnRemove);
			cart.SpotIdCollection = spotsInCart.ToArray();
			_mongoDBCollectionFactory.Collection<Cart>().Save(cart);
		}

		public Cart CreateCart(IElementIdentity elementIdentity)
		{
			throw new System.NotImplementedException();
		}

		private Cart FindFirstCart(IElementIdentity cartIdentity)
		{
			var cartId = cartIdentity.ToIdentity();
			return _mongoDBCollectionFactory.Collection<Cart>().FindOne(Query<Cart>.Where(e => e.CartId == cartId));
		}
	}
}
