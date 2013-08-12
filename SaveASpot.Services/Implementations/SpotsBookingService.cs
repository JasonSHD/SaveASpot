using SaveASpot.Core;
using SaveASpot.Core.Logging;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class SpotsBookingService : ISpotsBookingService
	{
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ILogger _logger;

		public SpotsBookingService(ISpotRepository spotRepository, ISpotQueryable spotQueryable, ILogger logger)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
			_logger = logger;
		}

		public bool BookingForCustomer(IElementIdentity spotIdentity, IElementIdentity customerIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(spotIdentity)).FirstOrDefault();
			if (spot == null)
			{
				_logger.Info("Try to map spot with identity <{0}> to customer <{1}>. But spot not found.", spotIdentity, customerIdentity);
				return false;
			}

			return _spotRepository.MapSpotToCustomer(spot, customerIdentity);
		}
	}
}