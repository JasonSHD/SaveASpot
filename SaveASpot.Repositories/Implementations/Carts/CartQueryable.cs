using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Carts;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Carts
{
	public sealed class CartQueryable : BasicMongoDBElementQueryable<Cart, ICartFilter>, ICartQueryable
	{
		public CartQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory)
			: base(mongoDBCollectionFactory)
		{
		}

		protected override ICartFilter BuildFilter(IMongoQuery query)
		{
			return new CartFilter(query);
		}

		public ICartFilter FilterByIdentity(IElementIdentity cartIdentity)
		{
			var cartId = cartIdentity.ToIdentity();
			return new CartFilter(Query<Cart>.Where(e => e.Id == cartId));
		}

		private sealed class CartFilter : MongoQueryFilter, ICartFilter
		{
			public CartFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}