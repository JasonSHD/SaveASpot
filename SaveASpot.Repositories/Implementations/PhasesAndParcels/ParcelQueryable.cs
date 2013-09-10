using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class ParcelQueryable : BasicMongoDBElementQueryable<Parcel, IParcelFilter>, IParcelQueryable
	{
		public ParcelQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory) : base(mongoDbCollectionFactory) { }

		public IParcelFilter All()
		{
			return new ParcelFilter(Query<Parcel>.Where(e => true));
		}

		public IParcelFilter ByPhase(IElementIdentity identity)
		{
			var phaseId = identity.ToIdentity();
			return new ParcelFilter(Query<Parcel>.Where(e => e.PhaseId == phaseId));
		}

		public IParcelFilter ByIdentities(IEnumerable<IElementIdentity> parcelsIdentities)
		{
			var parcelIds = parcelsIdentities.Select(e => e.ToIdentity());

			return new ParcelFilter(Query.And(Query<Parcel>.In(e => e.Id, parcelIds)));
		}

		protected override IParcelFilter BuildFilter(IMongoQuery query)
		{
			return new ParcelFilter(query);
		}

		private sealed class ParcelFilter : MongoQueryFilter, IParcelFilter
		{
			public ParcelFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}