using SaveASpot.Core;
using SaveASpot.Core.Validation;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class SpotValidateFactory : ISpotValidateFactory
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SpotValidateFactory(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public IValidator Available()
		{
			return Validator.
				For<SpotArg>().
				For(e => e.Spot.CustomerId, e => e.Not().IsEmpty(_elementIdentityConverter)).
				For(e => e.Spot.CustomerId, e => e.Not().IsEmpty(_elementIdentityConverter));
		}
	}
}