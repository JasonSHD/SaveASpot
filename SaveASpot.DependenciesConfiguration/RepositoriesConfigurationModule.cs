using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class RepositoriesConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IUserRepository>().To<IUserRepository>();
		}
	}
}