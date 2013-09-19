using SaveASpot.Core.Cart;
using SaveASpot.Repositories.Models;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Implementations;
using SaveASpot.Services.Implementations.Controllers;
using SaveASpot.Services.Implementations.TypeConverters;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels;
using SaveASpot.ViewModels.PhasesAndParcels;
using Cart = SaveASpot.Core.Cart.Cart;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class TypeConverterModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<ITypeConverter<SiteCustomer, CustomerViewModel>>().To<CustomerTypeConverter>();
			Bind<ITypeConverter<Phase, PhaseViewModel>>().To<PhaseTypeConverter>();
			Bind<ITypeConverter<Repositories.Models.Security.Cart, Cart>>().To<CartConverter>();
			Bind<ITypeConverter<SpotPhaseContainer, SpotElement>>().To<SpotPhaseContainerConverter>();
			Bind<ITypeConverter<Spot, SpotViewModel>>().To<SpotTypeConverter>();
		}
	}
}