using System.Reflection;
using System.Web.Mvc;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Logging.Implementation;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Core.Web.Mvc.ViewExtensions;
using SaveASpot.Repositories.Implementations.Logging;
using SaveASpot.Services.Implementations.Security;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class CoreConfigurationModule : Ninject.Modules.NinjectModule
	{
		private readonly Assembly _viewExtensionsAssembly;

		public CoreConfigurationModule(Assembly viewExtensionsAssembly)
		{
			_viewExtensionsAssembly = viewExtensionsAssembly;
		}

		public override void Load()
		{
			Bind<IControllerFactory>().To<ControllerFactory>();
			Bind<IWebAuthentication>().To<WebAuthentication>();
			Bind<IConfigurationManager>().To<ConfigurationManager>();
			Bind<IPasswordHash>().To<PasswordHash>();
			Bind<IActionFilter>().To<ViewPageInitializerFilter>();
			Bind<IActionFilter>().To<TabActionFilter>();
			Bind<IActionFilter>().To<CustomerActionFilter>();
			Bind<IActionFilter>().To<ViewExtensionFilter>();
			Bind<ITabElementFilter>().To<RoleTabElementFilter>();
			Bind<ILogConfiguration>().To<LogConfiguration>();
			Bind<ILogger>().To<Logger>();
			Bind<ILogAppender>().To<LogAppender>();
			Bind<ICurrentCustomer>().To<CurrentCustomer>();
			Bind<ModelMetadataProvider>().To<ResourceDataAnnotationsModelMetadataProvider>();
			Bind<IApplicationConfiguration>().To<ApplicationConfiguration>();
			Bind<IAnonymUser>().To<UserFactory>();
			Bind<IViewExtensionsBuilder>().To<ViewExtensionsBuilder>();
			Bind<IViewExtensionsFinder>().ToMethod(c => new NInjectViewExtensionsFinder(Kernel, _viewExtensionsAssembly));
			Bind<IElementIdentityConverter>().To<TypeElementIdentityConverter>().WhenInjectedInto<ViewExtensionFilter>();
			Bind<ICurrentSessionIdentity>().To<CurrentSessionIdentity>();
		}
	}
}