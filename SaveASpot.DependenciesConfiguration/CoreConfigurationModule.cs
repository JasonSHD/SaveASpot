using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class CoreConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IControllerFactory>().To<ControllerFactory>();
		}
	}
}
