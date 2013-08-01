using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.Sponsors
{
	public sealed class SponsorQueryable  :ISponsorQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public SponsorQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public ISponsorFilter All()
		{
			return new SponsorFilter(Query<Sponsor>.Where(e => true));
		}

		public ISponsorFilter ByName(string name)
		{
			return new SponsorFilter(Query<Sponsor>.Where(e => e.CompanyName == name));
		}

		public ISponsorFilter ByUrl(string url)
		{
			return new SponsorFilter(Query<Sponsor>.Where(e => e.Url == url));
		}

		public IEnumerable<Sponsor> Find(ISponsorFilter sponsorFilter)
		{
			return _mongoDbCollectionFactory.Collection<Sponsor>().Find(MongoQueryFilter.Convert(sponsorFilter).MongoQuery);
		}

		private sealed class SponsorFilter : MongoQueryFilter, ISponsorFilter
		{
			public SponsorFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}
