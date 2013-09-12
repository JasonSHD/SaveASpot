using System.Linq;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class CustomerTypeConverter : ITypeConverter<SiteCustomer, CustomerViewModel>
	{
		private readonly IUserQueryable _userQueryable;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public CustomerTypeConverter(IUserQueryable userQueryable, IElementIdentityConverter elementIdentityConverter)
		{
			_userQueryable = userQueryable;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public CustomerViewModel Convert(SiteCustomer source)
		{
			var user = _userQueryable.Filter(e => e.FilterById(_elementIdentityConverter.ToIdentity(source.UserId))).First();

			return new CustomerViewModel
							 {
								 Email = user.Email,
								 Identity = _elementIdentityConverter.ToIdentity(source.Id),
								 Username = user.Username
							 };
		}
	}
}