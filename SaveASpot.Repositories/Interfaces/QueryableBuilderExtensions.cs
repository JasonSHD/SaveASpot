using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Repositories.Interfaces
{
	public static class QueryableBuilderExtensions
	{
		public static T First<T, TFilter, TQueryable>(this QueryableBuilder<T, TFilter, TQueryable> source)
			where TFilter : class
			where TQueryable : IElementQueryable<T, TFilter>
		{
			return source.Find().First();
		}

		public static T FirstOrDefault<T, TFilter, TQueryable>(this QueryableBuilder<T, TFilter, TQueryable> source)
			where TFilter : class
			where TQueryable : IElementQueryable<T, TFilter>
		{
			return source.Find().FirstOrDefault();
		}

		public static IList<T> ToList<T, TFilter, TQueryable>(this QueryableBuilder<T, TFilter, TQueryable> source)
			where TFilter : class
			where TQueryable : IElementQueryable<T, TFilter>
		{
			return source.Find().ToList();
		}
	}
}