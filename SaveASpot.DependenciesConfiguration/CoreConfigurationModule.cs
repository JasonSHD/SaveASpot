﻿using System.Web.Mvc;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class CoreConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IControllerFactory>().To<ControllerFactory>();
			Bind<IWebAuthentication>().To<WebAuthentication>();
			Bind<IConfigurationManager>().To<ConfigurationManager>();
			Bind<IPasswordHash>().To<PasswordHash>();
			Bind<IActionFilter>().To<ViewPageInitializerFilter>();
			Bind<IActionFilter>().To<TabDescriptionActionFilter>();
		}
	}
}
