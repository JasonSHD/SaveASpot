using SaveASpot.Core;
using SaveASpot.Core.Cart;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class SpotPhaseContainerConverter : ITypeConverter<SpotPhaseContainer, SpotElement>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SpotPhaseContainerConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public SpotElement Convert(SpotPhaseContainer source)
		{
			return new SpotElement(_elementIdentityConverter.ToIdentity(source.Spot.Id),
														 new PhaseElement(_elementIdentityConverter.ToIdentity(source.Phase.Id),
															 source.Phase.SpotPrice,
															 source.Phase.PhaseName));
		}
	}
}