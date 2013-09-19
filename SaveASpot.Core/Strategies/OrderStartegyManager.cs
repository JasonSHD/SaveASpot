using System;
using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Core.Strategies
{
	public sealed class OrderStartegyManager<TArg, TResult> : IStrategyManager<TArg, TResult>
	{
		private readonly IEnumerable<IStrategy<TArg, TResult>> _strategies;

		public OrderStartegyManager(IEnumerable<IStrategy<TArg, TResult>> strategies)
		{
			_strategies = strategies;
		}

		public IEnumerable<IMethodResult<TResult>> Execute(TArg arg)
		{
			return new StrategyExecuter<TArg, TResult>().Execute(_strategies.OrderBy(GetOrder), arg);
		}

		private int GetOrder(IStrategy<TArg, TResult> strategy)
		{
			var strategyOrder = strategy as IStrategyOrder;
			if (strategyOrder != null)
			{
				return strategyOrder.Order;
			}

			var orderAttributes =
				strategy.GetType().GetCustomAttributes(false).OfType<StrategyOrderAttribute>().ToList();

			if (orderAttributes.Any())
			{
				return orderAttributes.First().Order;
			}

			return 0;
		}
	}

	public interface IStrategyOrder
	{
		int Order { get; }
	}

	public sealed class StrategyOrderAttribute : Attribute
	{
		private readonly int _order;
		public int Order { get { return _order; } }

		public StrategyOrderAttribute(int order)
		{
			_order = order;
		}
	}
}