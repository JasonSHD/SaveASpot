﻿using MongoDB.Bson;
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
	}
}
