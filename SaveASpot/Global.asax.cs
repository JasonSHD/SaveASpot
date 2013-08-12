using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SaveASpot.Areas.Setup.Controllers.Artifacts;
using SaveASpot.Controllers;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Logging.Implementation;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.DependenciesConfiguration;
using SaveASpot.Repositories.Implementations;
using SaveASpot.Repositories.Implementations.Logging;

namespace SaveASpot
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private readonly ILogger _logger;
		private readonly StandardKernel kernel;

		public MvcApplication()
		{
			kernel = new StandardKernel(new CoreConfigurationModule(), new ServicesConfigurationModule(), new RepositoriesConfigurationModule(), new SetupAreaConfigurationModule());
			_logger = kernel.Get<ILogger>();
		}


		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Error()
		{
			var applicationConfiguration = kernel.Get<IApplicationConfiguration>();

			var lastError = Server.GetLastError();
			_logger.Error("An unhandled exception has occured!", lastError);
			if (applicationConfiguration.Mode == ApplicationMode.Release)
			{
				Server.ClearError();

				Response.Clear();
				var routeData = new RouteData();
				routeData.Values["controller"] = "ErrorPage";
				routeData.Values["action"] = "Index";
				IController controller = kernel.Get<ErrorPageController>();
				var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
				controller.Execute(rc);
			}
		}

		protected void Application_Start()
		{
			kernel.Bind<ITabDescriptionControllerTypes>().ToMethod(c => new TabDescriptionControllerTypes(typeof(MapController).Assembly));
			kernel.Bind<IControllerTypesFinder>().ToMethod(c => new ControllerTypesFinder(typeof(MapController).Assembly));
			GlobalFilters.Filters.Add(kernel.Get<MvcAuthorizeFilter>());
			foreach (var actionFilter in kernel.GetAll<IActionFilter>())
			{
				GlobalFilters.Filters.Add(actionFilter);
			}

			ModelBinders.Binders.Add(typeof(IElementIdentity), kernel.Get<ElementIdentityPropertyBinder>());

			ControllerBuilder.Current.SetControllerFactory(kernel.Get<IControllerFactory>());
			ModelMetadataProviders.Current = kernel.Get<ModelMetadataProvider>();

			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());
		}
	}
}