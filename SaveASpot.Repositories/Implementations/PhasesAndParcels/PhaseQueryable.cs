using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class PhaseQueryable : IPhaseQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public PhaseQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public IPhaseFilter All()
		{
			return new PhaseFilter(Query<Phase>.Where(e => true));
		}

		public IEnumerable<Phase> Find(IPhaseFilter phaseFilter)
		{
			return _mongoDbCollectionFactory.Collection<Phase>().Find(MongoQueryFilter.Convert(phaseFilter).MongoQuery);
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