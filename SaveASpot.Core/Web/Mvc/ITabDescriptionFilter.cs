using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc
{
	public interface ITabDescriptionFilter
	{
		IEnumerable<TabDescription> Filter(IEnumerable<TabDescription> tabDescriptions);
	}
}