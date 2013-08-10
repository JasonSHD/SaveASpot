using System;
using System.Collections.Generic;

namespace SaveASpot.Repositories.Interfaces
{
	public sealed class QueryableBuilder<T, TFilter, TQueryable>
		where TFilter : class
		where TQueryable : IElementQueryable<T, TFilter>
	{
		private readonly TQueryable _elementQueryable;
		private TFilter Filter { get; set; }

		public QueryableBuilder(TQueryable elementQueryable)
		{
			_elementQueryable = elementQueryable;

			Filter = null;
		}

		public QueryableBuilder<T, TFilter, TQueryable> And(Func<TQueryable, TFilter> filter)
		{
			if (Filter == null)
			{
				Filter = filter(_elementQueryable);
			}
			else
			{
				Filter = _elementQueryable.And(Filter, filter(_elementQueryable));
			}

			return this;
		}

		public IEnumerable<T> Find()
		{
			return _elementQueryable.Find(Filter);
		}
	}
}