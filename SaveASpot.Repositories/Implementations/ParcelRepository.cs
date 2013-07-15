using MongoDB.Bson;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class ParcelRepository : IParcelRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public ParcelRepository(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDbCollectionFactory;
		}

		public Parcel AddParcel(Parcel parcel)
		{
			parcel.Id = ObjectId.GenerateNewId();
			_mongoDbCollectionFactory.Collection<Parcel>().Insert(parcel);

			return parcel;
		}
	}
}
