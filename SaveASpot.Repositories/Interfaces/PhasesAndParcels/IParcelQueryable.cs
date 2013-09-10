using System;
using System.Collections.Generic;
using SaveASpot.Core;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Interfaces.PhasesAndParcels
{
	public interface IParcelQueryable : IElementQueryable<Parcel, IParcelFilter>
	{
		IParcelFilter All();
		IParcelFilter ByPhase(IElementIdentity identity);
		IParcelFilter ByIdentities(IEnumerable<IElementIdentity> parcelsIdentities);
	}

	public static class ParcelQueryableExtensions
	{
		public static QueryableBuilder<Parcel, IParcelFilter, IParcelQueryable> Filter(this IParcelQueryable source,
																																									 Func<IParcelQueryable, IParcelFilter>
																																										 filter)
		{
			return new QueryableBuilder<Parcel, IParcelFilter, IParcelQueryable>(source).And(filter);
		}
	}
}