using Ninject.Modules;

namespace SaveASpot.Areas.Setup.Controllers.Artifacts
{
	public sealed class SetupAreaConfigurationModule : NinjectModule
	{
		public override void Load()
		{
			Bind<ISetupConfiguration>().To<SetupConfiguration>();
		}
	}
}