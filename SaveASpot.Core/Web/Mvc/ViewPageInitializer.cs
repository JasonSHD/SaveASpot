using System;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewPageInitializer
	{
		private readonly IConfigurationManager _configurationManager;
		private readonly ICurrentUser _currentUser;
		private readonly IAnonymUser _anonymUser;

		public ViewPageInitializer(IConfigurationManager configurationManager, ICurrentUser currentUser, IAnonymUser anonymUser)
		{
			_configurationManager = configurationManager;
			_currentUser = currentUser;
			_anonymUser = anonymUser;
		}

		public void Initialize<TModel>(BaseViewPage<TModel> viewPage)
		{
			var isMinimiziScripts = _configurationManager.GetSettings("UseMinimizedScripts") ?? "false";

			viewPage.ViewConfiguration = new ViewConfiguration(isMinimiziScripts.Equals("true", StringComparison.InvariantCultureIgnoreCase), _currentUser.User, _anonymUser.User);
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