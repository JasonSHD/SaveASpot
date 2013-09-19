using System.Collections.Generic;

namespace SaveASpot.Core.Strategies
{
	public sealed class DefaultStrategyManager<TArg, TResult> : IStrategyManager<TArg, TResult>
	{
		private readonly IEnumerable<IStrategy<TArg, TResult>> _strategies;

		public DefaultStrategyManager(IEnumerable<IStrategy<TArg, TResult>> strategies)
		{
			_strategies = strategies;
		}

		public IEnumerable<IMethodResult<TResult>> Execute(TArg arg)
		{
			return new StrategyExecuter<TArg, TResult>().Execute(_strategies, arg);
		}
	}
}