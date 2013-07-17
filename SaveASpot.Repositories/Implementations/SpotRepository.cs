using MongoDB.Bson;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations
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
	}
}
