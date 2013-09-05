using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public sealed class ViewExtensionsBuilder : IViewExtensionsBuilder
	{
		private readonly IViewExtensionsFinder _viewExtensionsFinder;

		public ViewExtensionsBuilder(IViewExtensionsFinder viewExtensionsFinder)
		{
			_viewExtensionsFinder = viewExtensionsFinder;
		}

		public IEnumerable<IViewExtensionResult> CollectExtensions(IEnumerable<KeyValuePair<string, string>> args)
		{
			var result = new List<IViewExtensionResult>();
			var arguments = args.ToList();
			foreach (var viewExntesion in _viewExtensionsFinder.FindViewExtensions())
			{
				var requiredArgs = viewExntesion as IRequiredArgumentViewExtension;
				if (requiredArgs != null)
				{
					requiredArgs.Args = arguments;
				}

				result.AddRange(viewExntesion.CollectionExtensions());
			}

			return result;
		}
	}
}