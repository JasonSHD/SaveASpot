using System.Collections.Generic;

namespace SaveASpot.Core.Strategies
{
	public interface IStrategyManager<in TArg, out TResult>
	{
		IEnumerable<IMethodResult<TResult>> Execute(TArg arg);
	}
}