using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Web.Mvc.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class InitializeAnonymCustomerViewExtension : IViewExtension, IRequiredArgumentViewExtension
	{
		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			if (Args.Any(e => e.Key == "isAdmin" && !e.Value.Equals("true", StringComparison.CurrentCultureIgnoreCase)))
			{
				yield return new ViewExtensionResult("security/anonymCustomer", new object(), new ViewScriptsElementIdentity());
			}
		}

		public IEnumerable<KeyValuePair<string, string>> Args { get; set; }
	}
}