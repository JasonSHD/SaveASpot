using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using SaveASpot.Areas.Setup.Controllers.Artifacts;
using SaveASpot.Controllers;
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

			routes.MapRoute(
					"Default", // Route name
					"{controller}/{action}/{id}", // URL with parameters
					new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
			);
		}

		protected void Application_Start()
		{
			var kernel = new StandardKernel(new CoreConfigurationModule(), new ServicesConfigurationModule(), new RepositoriesConfigurationModule(), new SetupAreaConfigurationModule());
			kernel.Bind<ITabDescriptionControllerTypes>().ToMethod(c => new TabDescriptionControllerTypes(typeof(MapController).Assembly));
			GlobalFilters.Filters.Add(kernel.Get<MvcAuthorizeFilter>());
			foreach (var actionFilter in kernel.GetAll<IActionFilter>())
			{
				GlobalFilters.Filters.Add(actionFilter);
			}

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