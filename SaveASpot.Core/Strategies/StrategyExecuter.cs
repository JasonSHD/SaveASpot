using System.Collections.Generic;

namespace SaveASpot.Core.Strategies
{
	public sealed class StrategyExecuter<TArg, TResult>
	{
		public IEnumerable<IMethodResult<TResult>> Execute(IEnumerable<IStrategy<TArg, TResult>> strategies, TArg arg)
		{
			foreach (var strategy in strategies)
			{
				var result = strategy.Execute(arg);

				yield return result;

				var strategyResult = result as IStrategyResult;
				if (strategyResult != null && strategyResult.IsBreak)
				{
					break;
				}
			}
		}
	}
}