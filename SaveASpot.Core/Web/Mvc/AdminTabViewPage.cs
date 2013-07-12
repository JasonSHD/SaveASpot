using System.Collections.Generic;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public abstract class AdminTabViewPage<TModel> : WebViewPage<TModel>
	{
		public IEnumerable<TabDescription> TabDescriptions { get { return TabDescription.GetDescriptions(ViewBag); } }
	}
}