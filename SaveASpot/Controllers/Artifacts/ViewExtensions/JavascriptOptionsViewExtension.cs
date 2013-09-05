using System;
using System.Collections.Generic;
using SaveASpot.Core.Configuration;
using SaveASpot.Core.Web.Mvc.ViewExtensions;
using SaveASpot.ViewModels.ViewExtensions;

namespace SaveASpot.Controllers.Artifacts.ViewExtensions
{
	public sealed class JavascriptOptionsViewExtension : IViewExtension
	{
		private readonly IConfigurationManager _configurationManager;

		public JavascriptOptionsViewExtension(IConfigurationManager configurationManager)
		{
			_configurationManager = configurationManager;
		}

		public IEnumerable<IViewExtensionResult> CollectionExtensions()
		{
			var useMinimizedScripts = _configurationManager.GetSettings("UseMinimizedScripts") ?? "false";

			yield return new ViewExtensionResult("JavascriptResources", new JavascriptViewModel { UseMinimizedScript = useMinimizedScripts.Equals("true", StringComparison.CurrentCultureIgnoreCase) }, new JavascriptOptionsElementIdentity());
		}
	}
}