using MongoDB.Driver;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;

namespace SaveASpot.Repositories.Implementations.PhasesAndParcels
{
	public sealed class PhaseFilter : MongoQueryFilter, IPhaseFilter
	{
		public PhaseFilter(IMongoQuery mongoQuery)
			: base(mongoQuery)
		{
		}
	}
}