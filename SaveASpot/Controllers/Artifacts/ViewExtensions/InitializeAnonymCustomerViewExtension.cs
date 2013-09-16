using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;
using SaveASpot.Services.Interfaces;
using SaveASpot.ViewModels.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeAnonymCustomerViewExtension : IViewExtension, IRequiredArgumentViewExtension
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICurrentCart _currentCart;

		public InitializeAnonymCustomerViewExtension(ICurrentUser currentUser, ICurrentCart currentCart)
		{
			_currentUser = currentUser;
			_currentCart = currentCart;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (Args.All(e => e.Key != "isAdmin") && _currentUser.User.IsAnonym())
			{
				yield return new ViewExtensionResult("security/anonymcustomer", new UserCartViewModel
																																					{
																																						Cart = _currentCart.Cart,
																																						User = _currentUser.User
																																					}, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/customer", new object(), new ViewJavascriptConfigurationElementIdentity());
			}
		}

		public IEnumerable<KeyValuePair<string, string>> Args { get; set; }
	}
}