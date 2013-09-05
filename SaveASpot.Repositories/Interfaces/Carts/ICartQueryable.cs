using System;
using SaveASpot.Core;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Interfaces.Carts
{
	public interface ICartQueryable : IElementQueryable<Cart, ICartFilter>
	{
		ICartFilter FilterByIdentity(IElementIdentity cartIdentity);
	}

	public static class CartQueryableExtensions
	{
		public static QueryableBuilder<Cart, ICartFilter, ICartQueryable> Filter(this ICartQueryable source, Func<ICartQueryable, ICartFilter> filter)
		{
			return new QueryableBuilder<Cart, ICartFilter, ICartQueryable>(source).And(filter);
		}
	}
}