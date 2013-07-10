using SaveASpot.Core.Configuration;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewPageInitializer
	{
		private readonly IConfigurationManager _configurationManager;

		public ViewPageInitializer(IConfigurationManager configurationManager)
		{
			_configurationManager = configurationManager;
		}

		public void Initialize<TModel>(BaseViewPage<TModel> viewPage)
		{
			viewPage.ViewConfiguration = new ViewConfiguration(true);
		}

		public static void SetInitializer(dynamic viewBag, ViewPageInitializer viewPageInitializer)
		{
			viewBag.BaseViewPageInitialize = viewPageInitializer;
		}

		public static ViewPageInitializer GetInitializer(dynamic viewBag)
		{
			return viewBag.BaseViewPageInitialize;
		}
	}
}