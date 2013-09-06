using System;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseQueryable : IElementQueryable<Phase, IPhaseFilter>
	{
		IPhaseFilter All();
		IPhaseFilter ByName(string name);
	}

	public static class PhaseQueryableExtensions
	{
		public static QueryableBuilder<Phase, IPhaseFilter, IPhaseQueryable> Filter(this IPhaseQueryable source, Func<IPhaseQueryable, IPhaseFilter> filter)
		{
			return new QueryableBuilder<Phase, IPhaseFilter, IPhaseQueryable>(source).And(filter);
		}
	}
}