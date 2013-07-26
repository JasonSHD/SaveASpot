using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class ParcelQueryable : IParcelQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public ParcelQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public IParcelFilter All()
		{
			return new ParcelFilter(Query<Parcel>.Where(e => true));
		}

		public IParcelFilter ByPhase(string identity)
		{
			return new ParcelFilter(Query<Parcel>.Where(e => true));
		}

		public IEnumerable<Parcel> Find(IParcelFilter filter)
		{
			return _mongoDbCollectionFactory.Collection<Parcel>().Find(MongoQueryFilter.Convert(filter).MongoQuery);
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