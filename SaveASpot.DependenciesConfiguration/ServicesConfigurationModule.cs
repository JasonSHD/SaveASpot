using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Implementations;
using SaveASpot.Services.Implementations.Controllers;
using SaveASpot.Services.Implementations.Security;
using SaveASpot.Services.Implementations.Validators.Security;
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
			Bind<IUserValidateFactory>().To<UserValidateFactory>();
			Bind<ICurrentUser>().To<CurrentUser>();
			Bind<IRoleHarvester>().To<RoleHarvester>();
			Bind<ITextService>().To<TextService>();
			Bind<IAuthorizeManager>().To<AuthorizeManager>();
		}
	}
}