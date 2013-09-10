using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeAnonymCustomerViewExtension : IViewExtension, IRequiredArgumentViewExtension
	{
		private readonly ICurrentUser _currentUser;

		public InitializeAnonymCustomerViewExtension(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (Args.All(e => e.Key != "isAdmin") && _currentUser.User.IsAnonym())
			{
				yield return new ViewExtensionResult("security/anonymCustomer", _currentUser.User, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/customer", new object(), new ViewJavascriptConfigurationElementIdentity());
			}
		}

		public IEnumerable<KeyValuePair<string, string>> Args { get; set; }
	}
}