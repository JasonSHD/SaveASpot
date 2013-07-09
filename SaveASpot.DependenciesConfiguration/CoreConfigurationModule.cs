using System.Web.Mvc;
using Ninject;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class CoreConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IKernel>().ToConstant(Kernel);
			Bind<IControllerFactory>().To<ControllerFactory>();
		}
	}
}
