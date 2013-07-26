using SaveASpot.Core;
using SaveASpot.Core.Security;
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
			Bind<ICustomersControllerService>().To<CustomersControllerService>();
			Bind<ICurrentUser>().To<CurrentUser>();
			Bind<IRoleFactory>().To<RoleFactory>();
			Bind<ITextService>().To<TextService>();
			Bind<IParcelsService>().To<ArcgisParcelsService>();
			Bind<ISpotsService>().To<ArcgisSpotsService>();
			Bind<IAuthorizeManager>().To<AuthorizeManager>();
			Bind<ISetupUsersControllerService>().To<SetupUsersControllerService>();
			Bind<IParcelsControllerService>().To<ParcelsControllerService>();
			Bind<IPhasesControllerService>().To<PhasesControllerService>();
			Bind<ISpotsControllerService>().To<SpotsControllerService>();
			Bind<IUploadPhasesAndParcelsControllerService>().To<UploadPhasesAndParcelsControllerService>();
			Bind<ITextParserEngine>().To<TextParserEngine>();
		}
	}
}