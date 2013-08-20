using System;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public static class SpotQueryableExtensions
	{
		public static QueryableBuilder<Spot, ISpotFilter, ISpotQueryable> Filter(this ISpotQueryable source,
		                                                                         Func<ISpotQueryable, ISpotFilter> filter)
		{
			return new QueryableBuilder<Spot, ISpotFilter, ISpotQueryable>(source).And(filter);
		}
	}
}