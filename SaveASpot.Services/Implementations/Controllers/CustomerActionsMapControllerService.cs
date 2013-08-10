using System.Linq;
using MongoDB.Bson;
using SaveASpot.Core.Security;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CustomerActionsMapControllerService : ICustomerActionsMapControllerService
	{
		private readonly ICurrentCustomer _currentCustomer;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ISpotRepository _spotRepository;

		public CustomerActionsMapControllerService(ICurrentCustomer currentCustomer, ISpotQueryable spotQueryable, ISpotRepository spotRepository)
		{
			_currentCustomer = currentCustomer;
			_spotQueryable = spotQueryable;
			_spotRepository = spotRepository;
		}

		public bool BookingSpots(string[] identities)
		{
			foreach (string identity in identities)
			{
				var spot = _spotQueryable.Find(_spotQueryable.ByIdentity(identity)).FirstOrDefault();

				if (spot == null || spot.CustomerId != ObjectId.Empty)
				{
					continue;
				}

				ObjectId customerId;
				if (ObjectId.TryParse(_currentCustomer.Customer.Identity, out customerId))
				{
					spot.CustomerId = customerId;
					_spotRepository.Update(spot);
				}
			}


			return false;
		}
	}
}