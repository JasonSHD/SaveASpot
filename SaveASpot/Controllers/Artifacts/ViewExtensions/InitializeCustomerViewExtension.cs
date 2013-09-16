﻿using System.Collections.Generic;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc.ViewExtensions;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Security;
using SaveASpot.ViewModels.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeCustomerViewExtension : IViewExtension
	{
		private readonly ICurrentUser _currentUser;
		private readonly ICurrentCustomer _currentCustomer;
		private readonly ICurrentCart _currentCart;
		private readonly IUserFactory _userFactory;

		public InitializeCustomerViewExtension(ICurrentUser currentUser,
			ICurrentCustomer currentCustomer,
			ICurrentCart currentCart,
			IUserFactory userFactory)
		{
			_currentUser = currentUser;
			_currentCustomer = currentCustomer;
			_currentCart = currentCart;
			_userFactory = userFactory;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (_currentUser.User.IsCustomer())
			{
				yield return new ViewExtensionResult("security/customer", new CustomerCartViewModel
																																		{
																																			Cart = _currentCart.Cart,
																																			Customer = _currentCustomer.Customer,
																																			Anonym = _userFactory.AnonymUser()
																																		}, new ViewScriptsElementIdentity());
				yield return new ViewExtensionResult("configuration/customer", new { }, new ViewJavascriptConfigurationElementIdentity());
			}
		}
	}
}