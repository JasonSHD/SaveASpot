using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public interface IViewExtensionsBuilder
	{
		IEnumerable<IViewExtensionResult> CollectExtensions(IEnumerable<KeyValuePair<string, string>> args);
	}
}