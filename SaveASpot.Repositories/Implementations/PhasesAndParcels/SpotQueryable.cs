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

		public ISpotFilter ByArea(decimal area)
		{
			var areaPlusOne = area + (decimal)0.01;

			return new SpotFilter(Query.And(Query<Spot>.Where(e => e.SpotArea >= area), Query<Spot>.Where(e => e.SpotArea <= areaPlusOne)));
		}

		public ISpotFilter And(ISpotFilter left, ISpotFilter right)
		{
			return new SpotFilter(Query.And(MongoQueryFilter.Convert(left).MongoQuery, MongoQueryFilter.Convert(right).MongoQuery));
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