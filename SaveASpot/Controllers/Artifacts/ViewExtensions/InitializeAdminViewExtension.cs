using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeAdminViewExtension : IViewExtension, IRequiredArgumentViewExtension
	{
		private readonly ICurrentUser _currentUser;

		public InitializeAdminViewExtension(ICurrentUser currentUser)
		{
			_currentUser = currentUser;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (_currentUser.User.IsAdmin() || Args.Any(e => e.Key.Equals("isAdmin", StringComparison.CurrentCultureIgnoreCase) && e.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase)))
			{
				yield return new ViewExtensionResult("security/admin", _currentUser.User, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/admin", null, new ViewJavascriptConfigurationElementIdentity());
			}
		}

		public IEnumerable<KeyValuePair<string, string>> Args { private get; set; }
	}
}