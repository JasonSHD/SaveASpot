using MongoDB.Bson;
using MongoDB.Driver.Builders;
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

		public bool Remove(string identity)
		{
			ObjectId id;

			if (ObjectId.TryParse(identity, out id))
			{
				_mongoDbCollectionFactory.Collection<Spot>().Remove(Query<Spot>.Where(e => e.Id == id));

				return true;
			}

			return false;
		}
	}
}