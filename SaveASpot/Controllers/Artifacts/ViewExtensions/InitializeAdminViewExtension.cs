using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeAdminViewExtension : IViewExtension, IRequiredArgumentViewExtension
	{
		private readonly ICurrentUser _currentUser;
		private readonly IUserFactory _userFactory;

		public InitializeAdminViewExtension(ICurrentUser currentUser,
			IUserFactory userFactory)
		{
			_currentUser = currentUser;
			_userFactory = userFactory;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (_currentUser.User.IsAdmin() || Args.Any(e => e.Key.Equals("isAdmin", StringComparison.CurrentCultureIgnoreCase) && e.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase)))
			{
				yield return new ViewExtensionResult("security/admin", new AdminViewModel
																																 {
																																	 User = _currentUser.User,
																																	 Anonym = _userFactory.AnonymUser()
																																 }, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/admin", null, new ViewJavascriptConfigurationElementIdentity());
			}
		}

		public IEnumerable<KeyValuePair<string, string>> Args { private get; set; }
	}
}