using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class SpotRepository : ISpotRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public SpotRepository(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public Spot AddSpot(Spot spot)
		{
			spot.Id = ObjectId.GenerateNewId();
			_mongoDbCollectionFactory.Collection<Spot>().Insert(spot);

			return spot;
		}

		public bool Remove(IEnumerable<IElementIdentity> identity)
		{
			var idCollection = identity.Select(e => e.ToIdentity());

			_mongoDbCollectionFactory.Collection<Spot>().Remove(Query<Spot>.In(e => e.Id, idCollection));

			return true;
		}

		public bool Update(Spot spot)
		{
			_mongoDbCollectionFactory.Collection<Spot>().Save(spot);

			return true;
		}

		public bool MapSpotToCustomer(Spot spot, IElementIdentity customerIdentity, decimal? spotPrice)
		{
			spot.CustomerId = customerIdentity.ToIdentity();
			spot.SpotPrice = spotPrice;
			_mongoDbCollectionFactory.Collection<Spot>().Save(spot);

			return true;
		}

		public bool MapSpotToSponsor(Spot spot, IElementIdentity sponsorIdentity)
		{
			spot.SponsorId = sponsorIdentity.ToIdentity();
			_mongoDbCollectionFactory.Collection<Spot>().Save(spot);

			return true;
		}

		public bool RemoveMap(Spot spot)
		{
			spot.CustomerId = ObjectId.Empty;
			_mongoDbCollectionFactory.Collection<Spot>().Save(spot);

			return true;
		}
	}
}
