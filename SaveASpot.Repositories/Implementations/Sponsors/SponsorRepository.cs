using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.Sponsors;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.Sponsors
{
	public sealed class SponsorRepository : ISponsorRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public SponsorRepository(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public Sponsor AddSponsor(Sponsor sponsor)
		{
			sponsor.Id = ObjectId.GenerateNewId();
			_mongoDbCollectionFactory.Collection<Sponsor>().Insert(sponsor);

			return sponsor;
		}

		public bool Remove(string identity)
		{
			ObjectId id;

			if (ObjectId.TryParse(identity, out id))
			{
				_mongoDbCollectionFactory.Collection<Sponsor>().Remove(Query<Sponsor>.Where(e => e.Id == id));
				return true;
			}

			return true;
		}
	}
}
