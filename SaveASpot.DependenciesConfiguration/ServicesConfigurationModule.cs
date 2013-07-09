using SaveASpot.Services.Implementations;
using SaveASpot.Services.Implementations.Controllers;
using SaveASpot.Services.Implementations.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class ServicesConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IAccountControllerService>().To<AccountControllerService>();
			Bind<IUserService>().To<UserService>();
			Bind<IUserHarvester>().To<UserHarvester>();
			Bind<ICurrentUser>().To<CurrentUser>();
			Bind<ITextService>().To<TextService>();
		}
	}
}