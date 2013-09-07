using System.Collections.Generic;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeCustomerViewExtension : IViewExtension
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICurrentCustomer _currentCustomer;

		public InitializeCustomerViewExtension(ICurrentUser currentUser, ICurrentCustomer currentCustomer)
		{
			_currentUser = currentUser;
			_currentCustomer = currentCustomer;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (_currentUser.User.IsCustomer())
			{
				yield return new ViewExtensionResult("security/customer", _currentCustomer.Customer, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/customer", new { }, new ViewJavascriptConfigurationElementIdentity());
			}
		}
	}
}