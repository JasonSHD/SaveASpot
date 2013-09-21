using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
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

		public bool Remove(IEnumerable<IElementIdentity> identities)
		{
			var idCollection = identities.Select(e => e.ToIdentity());

			_mongoDbCollectionFactory.Collection<Parcel>().Remove(Query<Parcel>.In(e => e.Id, idCollection));
			return true;
		}

		public IEnumerable<Parcel> GetAllParcelsByPhaseId(string phaseId)
		{
			var targetPhaseId = phaseId.ToIdentity();
			return _mongoDbCollectionFactory.Collection<Parcel>().Find(Query<Parcel>.Where(p => p.PhaseId == targetPhaseId));
		}
	}
}
