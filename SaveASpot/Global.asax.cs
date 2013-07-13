using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SaveASpot.Areas.Setup.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.DependenciesConfiguration;

namespace SaveASpot
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapRoute("LogOn", "logon", new { controller = "Account", action = "LogOn" });

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			var kernel = new StandardKernel(new CoreConfigurationModule(), new ServicesConfigurationModule(), new RepositoriesConfigurationModule(), new SetupAreaConfigurationModule());
			GlobalFilters.Filters.Add(kernel.Get<MvcAuthorizeFilter>());
			GlobalFilters.Filters.Add(kernel.Get<ViewPageInitializerFilter>());

			ControllerBuilder.Current.SetControllerFactory(kernel.Get<IControllerFactory>());

			AreaRegistration.RegisterAllAreas();

			RegisterGlobalFilters(GlobalFilters.Filters);
			RegisterRoutes(RouteTable.Routes);

			ViewEngines.Engines.Clear();
			ViewEngines.Engines.Add(new RazorViewEngine());
		}
	}
}