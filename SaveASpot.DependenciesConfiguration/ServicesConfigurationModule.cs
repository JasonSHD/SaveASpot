using SaveASpot.Core;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Repositories.Models;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Implementations;
using SaveASpot.Services.Implementations.Controllers;
using SaveASpot.Services.Implementations.Security;
using SaveASpot.Services.Implementations.Validators.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class ServicesConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<IAccountControllerService>().To<AccountControllerService>();
			Bind<IUserService>().To<UserService>();
			Bind<IUserFactory>().To<UserFactory>();
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
			Bind<ISponsorsService>().To<SponsorsService>();
			Bind<ISponsorsControllerService>().To<SponsorsControllerService>();
			Bind<IMapControllerService>().To<MapControllerService>();
			Bind<ICustomerService>().To<CustomersService>();
			Bind<ITypeConverter<SiteCustomer, CustomerViewModel>>().To<CustomerTypeConverter>();
			Bind<ISpotsBookingService>().To<SpotsBookingService>();
			Bind<ISpotValidateFactory>().To<SpotValidateFactory>();
			Bind<IConverter<Sponsor, SponsorViewModel>>().To<SponsorConverter>();
			Bind<IAdministratorActionsMapControllerService>().To<AdministratorActionsMapControllerService>();
			Bind<IParcelService>().To<ParcelService>();
			Bind<ISpotService>().To<SpotService>();
			Bind<ITypeConverter<Phase, PhaseViewModel>>().To<PhaseTypeConverter>();
			Bind<ICartControllerService>().To<CartControllerService>();
			Bind<ICurrentCart>().To<CurrentCart>();
			Bind<ICartService>().To<CartService>();
			Bind<ITypeConverter<Repositories.Models.Security.Cart, Core.Security.Cart>>().To<CartConverter>();
			Bind<ICheckoutControllerService>().To<CheckoutControllerService>();
			Bind<IStripeControllerService>().To<StripeControllerService>();			
		}
	}
}