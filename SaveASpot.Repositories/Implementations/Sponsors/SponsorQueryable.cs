using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.Sponsors
{
	public sealed class SponsorQueryable : BasicMongoDBElementQueryable<Sponsor, ISponsorFilter>, ISponsorQueryable
	{
		public SponsorQueryable(IMongoDBCollectionFactory mongoDbCollectionFactory)
			: base(mongoDbCollectionFactory)
		{
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

		protected override ISponsorFilter BuildFilter(IMongoQuery query)
		{
			return new SponsorFilter(query);
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
