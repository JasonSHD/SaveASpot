using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class PhaseQueryable : BasicMongoDBElementQueryable<Phase, IPhaseFilter>, IPhaseQueryable
	{
		public PhaseQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
			: base(mongoDbCollectionFactory)
		{
		}

		public IPhaseFilter All()
		{
			return new PhaseFilter(Query<Phase>.Where(e => true));
		}

		public IPhaseFilter ByName(string name)
		{
			return new PhaseFilter(Query<Phase>.Where(e => e.PhaseName == name));
		}

		public IPhaseFilter ByIdentity(IElementIdentity phaseIdentity)
		{
			var phaseId = phaseIdentity.ToIdentity();
			return new PhaseFilter(Query<Phase>.Where(e => e.Id == phaseId));
		}

		public IPhaseFilter ByIdentities(IEnumerable<IElementIdentity> phasesIdentities)
		{
			var phasesIds = phasesIdentities.Select(e => e.ToIdentity());

			return new PhaseFilter(Query.And(Query<Spot>.In(e => e.Id, phasesIds)));
		}

		protected override IPhaseFilter BuildFilter(IMongoQuery query)
		{
			return new PhaseFilter(query);
		}

		private sealed class PhaseFilter : MongoQueryFilter, IPhaseFilter
		{
			public PhaseFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}