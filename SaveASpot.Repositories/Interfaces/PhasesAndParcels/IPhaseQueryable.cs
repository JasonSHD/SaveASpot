using System;
using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IPhaseQueryable : IElementQueryable<Phase, IPhaseFilter>
	{
		IPhaseFilter All();
		IPhaseFilter ByName(string name);
		IPhaseFilter ByIdentity(IElementIdentity phaseIdentity);
		IPhaseFilter ByIdentities(IEnumerable<IElementIdentity> phasesIdentities);
	}

	public static class PhaseQueryableExtensions
	{
		public static QueryableBuilder<Phase, IPhaseFilter, IPhaseQueryable> Filter(this IPhaseQueryable source, Func<IPhaseQueryable, IPhaseFilter> filter)
		{
			return new QueryableBuilder<Phase, IPhaseFilter, IPhaseQueryable>(source).And(filter);
		}
	}
}