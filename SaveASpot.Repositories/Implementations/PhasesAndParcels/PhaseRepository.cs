using MongoDB.Bson;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
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

		public bool RemovePhase(string identity)
		{
			ObjectId phaseId;
			if (ObjectId.TryParse(identity, out phaseId))
			{
				_mongoDBCollectionFactory.Collection<Phase>().Remove(Query<Phase>.Where(e => e.Id == phaseId));

				return true;
			}

			return false;
		}

		public bool UpdatePhase(string identity, Phase phase)
		{
			var id = ObjectId.Parse(identity);

			var result = _mongoDBCollectionFactory.Collection<Phase>()
															 .Update(Query<Phase>.EQ(e => e.Id, id),
																			 Update<Phase>
																			 .Set(e => e.PhaseName, phase.PhaseName)
																			 .Set(e => e.SpotPrice, phase.SpotPrice)
																			 );

			return result.DocumentsAffected == 1;
		}
	}
}