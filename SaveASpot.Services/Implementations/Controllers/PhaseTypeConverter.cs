using SaveASpot.Core;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class PhaseTypeConverter : ITypeConverter<Phase, PhaseViewModel>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public PhaseTypeConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public PhaseViewModel Convert(Phase source)
		{
			return new PhaseViewModel { Identity = _elementIdentityConverter.ToIdentity(source.Id), Name = source.PhaseName, SpotPrice = source.SpotPrice };
		}
	}
}