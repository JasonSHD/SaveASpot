using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class SpotQueryable : ISpotQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public SpotQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public ISpotFilter All()
		{
			return new SpotFilter(Query<Spot>.Where(e => true));
		}

		public IEnumerable<Spot> Find(ISpotFilter spotFilter)
		{
			return _mongoDbCollectionFactory.Collection<Spot>().Find(MongoQueryFilter.Convert(spotFilter).MongoQuery);
		}

		private sealed class SpotFilter : MongoQueryFilter, ISpotFilter
		{
			public SpotFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}