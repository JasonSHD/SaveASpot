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
		private readonly ISpotValidateFactory _spotValidateFactory;

		public SpotsBookingService(ISpotRepository spotRepository, ISpotQueryable spotQueryable, ILogger logger, ISpotValidateFactory spotValidateFactory)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
			_logger = logger;
			_spotValidateFactory = spotValidateFactory;
		}

		public bool BookingForSponsor(IElementIdentity spotIdentity, IElementIdentity sponsorIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(spotIdentity)).FirstOrDefault();
			if (spot == null)
			{
				_logger.Info("Try to map spot with identity <{0}> to sponsor <{1}>. But spot not found.", spotIdentity, sponsorIdentity);
				return false;
			}

			var validateResult = _spotValidateFactory.Available().Validate(new SpotArg { Spot = spot });

			if (validateResult.IsValid)
			{
				return _spotRepository.MapSpotToSponsor(spot, sponsorIdentity);
			}

			return false;
		}
	}
}