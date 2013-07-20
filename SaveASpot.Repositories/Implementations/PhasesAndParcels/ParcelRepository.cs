using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
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

		public bool Remove(string identity)
		{
			ObjectId id;

			if (ObjectId.TryParse(identity, out id))
			{
				_mongoDbCollectionFactory.Collection<Parcel>().Remove(Query<Parcel>.Where(e => e.Id == id));
				return true;
			}

			return true;
		}
	}
}
