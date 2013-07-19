using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc
{
	public interface ITabElementFilter
	{
		IEnumerable<TabElement> Filter(IEnumerable<TabElement> filter);
	}
}