using System.Collections.Generic;

namespace SaveASpot.Repositories.Interfaces
{
	public interface IElementQueryable<T, TFilter>
	{
		TFilter And(TFilter left, TFilter right);
		IEnumerable<T> Find(TFilter filter);
	}
}
