using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public abstract class BaseViewPage<TModel> : WebViewPage<TModel>
	{
		public ViewConfiguration ViewConfiguration { get; set; }

		protected override void InitializePage()
		{
			base.InitializePage();
			var initializer = ViewPageInitializer.GetInitializer(ViewBag);
			initializer.Initialize<TModel>(this);
		}
	}
}