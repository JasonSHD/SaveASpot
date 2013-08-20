using System.Globalization;
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

		public bool UpdateSponsor(string identity, Sponsor sponsor)
		{
			ObjectId id = ObjectId.Parse(identity);
			var result = _mongoDbCollectionFactory.Collection<Sponsor>()
															 .Update(Query<Sponsor>.EQ(e => e.Id, id),
																			 Update<Sponsor>
																			 .Set(e => e.CompanyName, sponsor.CompanyName)
																			 .Set(e => e.Sentence, sponsor.Sentence)
																			 .Set(e => e.Url, sponsor.Url)
																			 .Set(e => e.Logo, sponsor.Logo));
			return result.DocumentsAffected == 1;
		}
	}
}
