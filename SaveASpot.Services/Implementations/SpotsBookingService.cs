using SaveASpot.Core;
using SaveASpot.Core.Logging;
using SaveASpot.Repositories.Interfaces;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class SpotsBookingService : ISpotsBookingService
	{
		private readonly ISpotRepository _spotRepository;
		private readonly ISpotQueryable _spotQueryable;
		private readonly ILogger _logger;
		private readonly ISpotValidateFactory _spotValidateFactory;
		private readonly ICustomerRepository _customerRepository;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SpotsBookingService(ISpotRepository spotRepository, ISpotQueryable spotQueryable, ILogger logger, ISpotValidateFactory spotValidateFactory, ICustomerRepository customerRepository, IElementIdentityConverter elementIdentityConverter)
		{
			_spotRepository = spotRepository;
			_spotQueryable = spotQueryable;
			_logger = logger;
			_spotValidateFactory = spotValidateFactory;
			_customerRepository = customerRepository;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public bool BookingForCustomer(IElementIdentity spotIdentity, IElementIdentity customerIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(spotIdentity)).FirstOrDefault();
			if (spot == null)
			{
				_logger.Info("Try to map spot with identity <{0}> to customer <{1}>. But spot not found.", spotIdentity, customerIdentity);
				return false;
			}

			var validateResult = _spotValidateFactory.Available().Validate(new SpotArg { Spot = spot });

			if (validateResult.IsValid)
			{
				_customerRepository.AddSpotToCart(customerIdentity, _elementIdentityConverter.ToIdentity(spot.Id));
				return _spotRepository.MapSpotToCustomer(spot, customerIdentity);
			}

			return false;
		}

		public bool RemoveBookedSpot(IElementIdentity bookedSpotIdentity, IElementIdentity customerIdentity)
		{
			var spot = _spotQueryable.Filter(e => e.ByIdentity(bookedSpotIdentity)).FirstOrDefault();

			if (spot == null)
			{
				_logger.Info("Try to map spot with identity <{0}> to customer <{1}>. But spot not found.", bookedSpotIdentity, customerIdentity);
				return false;
			}

			var validationResult =
				_spotValidateFactory.AvailableForRemove().Validate(new RemoveSpotArg { CustomerIdentity = customerIdentity, Spot = spot });

			if (validationResult.IsValid)
			{
				_customerRepository.RemoveSpotFromCart(customerIdentity, _elementIdentityConverter.ToIdentity(spot.Id));
				return _spotRepository.RemoveMap(spot);
			}

			return false;
		}
	}
}