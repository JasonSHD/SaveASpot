using SaveASpot.Core;
using SaveASpot.Repositories.Models;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class SponsorConverter : IConverter<Sponsor, SponsorViewModel>
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public SponsorConverter(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public SponsorViewModel Convert(Sponsor @from)
		{
			return new SponsorViewModel
							 {
								 Identity = _elementIdentityConverter.ToIdentity(@from.Identity),
								 CompanyName = @from.CompanyName,
								 Sentence = @from.Sentence,
								 Logo = @from.Logo,
								 Url = @from.Url
							 };
		}
	}
}