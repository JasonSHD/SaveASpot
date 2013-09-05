using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public interface IViewExtensionsFinder
	{
		IEnumerable<IViewExtension> FindViewExtensions();
	}
}