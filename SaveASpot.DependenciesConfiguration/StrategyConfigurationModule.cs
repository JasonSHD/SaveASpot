using SaveASpot.Core;
using SaveASpot.Core.Strategies;
using SaveASpot.Services.Implementations.Geocoding;
using SaveASpot.Services.Interfaces.Geocoding;

namespace SaveASpot.DependenciesConfiguration
{
	public sealed class StrategyConfigurationModule : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind(typeof(IStrategyManager<,>)).To(typeof(DefaultStrategyManager<,>));

			//square elements calculator
			Bind(typeof(IStrategyManager<,>)).To(typeof(OrderStartegyManager<,>)).WhenInjectedInto(typeof(SquareElementsCalculator));
			Bind<IStrategy<SquareStrategyArg, SquareElementsResult>>().To<AllSpotsStrategy>();
			Bind<IStrategy<SquareStrategyArg, SquareElementsResult>>().To<PhaseSpotsStrategy>();
			Bind<IStrategy<SquareStrategyArg, SquareElementsResult>>().To<NotFoundSpotsStrategy>();
			Bind<IStrategy<SquareStrategyArg, SquareElementsResult>>().To<PartSpotsStrategy>();
			Bind<IStrategy<SquareStrategyArg, SquareElementsResult>>().To<LastSpotsStrategy>();
			//square elements calculator
		}
	}
}