using System.Web.Mvc;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Logging.Implementation;
using SaveASpot.Core.Logging.Interfaces;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Repositories.Implementations.Logging;

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
			Bind<ILogConfiguration>().To<LogConfiguration>();
			Bind<IActionFilter>().To<TabDescriptionActionFilter>();
			Bind<ILogger>().To<Logger>();
			Bind<ILogAppender>().To<LogAppender>();
		}
	}
}
