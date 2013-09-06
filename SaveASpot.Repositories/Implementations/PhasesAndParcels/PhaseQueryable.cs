using MongoDB.Driver;
using MongoDB.Driver.Builders;
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