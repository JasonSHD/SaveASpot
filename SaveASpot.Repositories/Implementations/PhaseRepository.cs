using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class PhaseRepository : IPhaseRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public PhaseRepository(IMongoDBCollectionFactory mongoDbCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDbCollectionFactory;
		}

		public Phase AddPhase(Phase phase)
		{
			phase.Id = ObjectId.GenerateNewId();
			_mongoDBCollectionFactory.Collection<Phase>().Insert(phase);

			return phase;
		}

		public bool PhaseExists(Phase phase)
		{
			var query = Query<Phase>.EQ(e => e.PhaseName, phase.PhaseName);
			var fields = Fields.Include("PhaseName");
			var res = _mongoDBCollectionFactory.Collection<Phase>().Find(query).SetFields(fields).SetLimit(1).FirstOrDefault();
			return res != null;
		}
	}
}
