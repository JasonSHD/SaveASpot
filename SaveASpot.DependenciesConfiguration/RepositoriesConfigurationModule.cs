using SaveASpot.Repositories.Implementations;
using SaveASpot.Repositories.Implementations.Security;
using SaveASpot.Repositories.Interfaces.Security;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class RepositoriesConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IUserRepository>().To<UserRepository>();
			Bind<IUserQueryable>().To<UserQueryable>();
			Bind<IMongoDBCollectionFactory>().To<MongoDBProvider>();
			Bind<IMongoDBConfiguration>().To<MongoDBConfiguration>();
		}
	}
}