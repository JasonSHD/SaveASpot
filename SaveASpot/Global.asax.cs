using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SaveASpot.Areas.Setup.Controllers.Artifacts;
using SaveASpot.Controllers;
using SaveASpot.Controllers.Artifacts.ViewExtensions;
using SaveASpot.Core;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.DependenciesConfiguration;
using Stripe;

namespace SaveASpot
{
	public class MvcApplication : HttpApplication
	{
		private readonly ILogger _logger;
		private readonly StandardKernel _kernel;

		public MvcApplication()
		{
			_kernel = new StandardKernel(new CoreConfigurationModule(typeof(JavascriptOptionsViewExtension).Assembly), new ServicesConfigurationModule(), new RepositoriesConfigurationModule(), new SetupAreaConfigurationModule());
			_logger = _kernel.Get<ILogger>();
		}

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

			routes.MapRoute("Admin default page", "admin", new { controller = "Map", action = "Index", isAdmin = true });

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Map", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Error()
		{
			var applicationConfiguration = _kernel.Get<IApplicationConfiguration>();

			var lastError = Server.GetLastError();
			_logger.Error("An unhandled exception has occured!", lastError);
			if (applicationConfiguration.Mode == ApplicationMode.Release)
			{
				Server.ClearError();

				Response.Clear();
				var routeData = new RouteData();
				routeData.Values["controller"] = "ErrorPage";
				routeData.Values["action"] = "Index";
				IController controller = _kernel.Get<ErrorPageController>();
				var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
				controller.Execute(rc);
			}
		}

		protected void Application_Start()
		{
			_kernel.Bind<ITabDescriptionControllerTypes>().ToMethod(c => new TabDescriptionControllerTypes(typeof(MapController).Assembly));
			_kernel.Bind<IControllerTypesFinder>().ToMethod(c => new ControllerTypesFinder(typeof(MapController).Assembly));
			GlobalFilters.Filters.Add(_kernel.Get<MvcAuthorizeFilter>());
			foreach (var actionFilter in _kernel.GetAll<IActionFilter>())
			{
				GlobalFilters.Filters.Add(actionFilter);
			}

			StripeConfiguration.SetApiKey(_kernel.Get<ConfigurationManager>().GetSettings("StripeSecretKey"));

			ModelBinders.Binders.Add(typeof(IElementIdentity), _kernel.Get<ElementIdentityPropertyBinder>());

			ControllerBuilder.Current.SetControllerFactory(_kernel.Get<IControllerFactory>());
			ModelMetadataProviders.Current = _kernel.Get<ModelMetadataProvider>();

			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());
		}
	}
}