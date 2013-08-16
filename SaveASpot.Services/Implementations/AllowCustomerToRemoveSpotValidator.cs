using SaveASpot.Core;
using SaveASpot.Core.Validation;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class AllowCustomerToRemoveSpotValidator : BaseValidator<RemoveSpotArg>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public AllowCustomerToRemoveSpotValidator(IElementIdentityConverter elementIdentityConverter)
			: base("UnavailableAccess")
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		protected override bool IsValid(RemoveSpotArg input)
		{
			return _elementIdentityConverter.IsEqual(input.CustomerIdentity,
			                                         _elementIdentityConverter.ToIdentity(input.Spot.CustomerId));
		}
	}
}