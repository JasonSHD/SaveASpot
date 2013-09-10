using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
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

		public ISpotFilter ByParcels(IEnumerable<IElementIdentity> identities)
		{
			var objectIdCollection = identities.Select(identity => identity.ToIdentity()).ToList();

			return new SpotFilter(Query.And(Query<Spot>.In(e => e.ParcelId, objectIdCollection)));
		}

		public ISpotFilter ByIdentity(IElementIdentity identity)
		{
			ObjectId objectId = identity.ToIdentity();

			return new SpotFilter(Query<Spot>.Where(e => e.Id == objectId));
		}

		public ISpotFilter ByIdentities(IEnumerable<IElementIdentity> identities)
		{
			var spotIds = identities.Select(e => e.ToIdentity());

			return new SpotFilter(Query.And(Query<Spot>.In(e => e.Id, spotIds)));
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