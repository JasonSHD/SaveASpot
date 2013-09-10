using SaveASpot.Services.Implementations.Controllers.Checkout;
using SaveASpot.Services.Interfaces.Controllers.Checkout;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class CheckoutServicesConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<ISpotsControllerService>()
				.To<SpotsControllerService>();
			Bind<IUserInfoControllerService>().To<UserInfoControllerService>();
		}
	}
}