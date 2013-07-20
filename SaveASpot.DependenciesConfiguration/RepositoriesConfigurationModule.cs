using SaveASpot.Repositories.Implementations;
using SaveASpot.Repositories.Implementations.PhasesAndParcels;
using SaveASpot.Repositories.Implementations.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
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
			Bind<IPhaseRepository>().To<PhaseRepository>();
			Bind<IParcelRepository>().To<ParcelRepository>();
			Bind<IPhaseQueryable>().To<PhaseQueryable>();
			Bind<ISpotRepository>().To<SpotRepository>();
			Bind<IParcelQueryable>().To<ParcelQueryable>();
			Bind<ISpotQueryable>().To<SpotQueryable>();
		}
	}
}