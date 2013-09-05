using System.Collections.Generic;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public sealed class ViewExtensionExecuter : IViewExtensionExecuter
	{
		private const string ViewExtensionSubFolder = "ViewExtensions";
		private readonly IEnumerable<IViewExtensionResult> _viewExtensionResults;
		private readonly ViewContext _viewContext;
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ViewExtensionExecuter(IEnumerable<IViewExtensionResult> viewExtensionResults, ViewContext viewContext, IElementIdentityConverter elementIdentityConverter)
		{
			_viewExtensionResults = viewExtensionResults;
			_viewContext = viewContext;
			_elementIdentityConverter = elementIdentityConverter;
		}

		public void Execute(IElementIdentity identity)
		{
			foreach (var viewExtensionResult in _viewExtensionResults)
			{
				if (!_elementIdentityConverter.IsEqual(identity, viewExtensionResult.RenderViewExtensionIdentity))
				{
					continue;
				}

				var viewName = ViewExtensionSubFolder + "\\" + viewExtensionResult.ViewName;
				var newViewContext = new ViewContext(_viewContext, _viewContext.View,
				                                     new ViewDataDictionary(viewExtensionResult.Model), _viewContext.TempData,
				                                     _viewContext.Writer);
				IView view = ViewEngines.Engines.FindPartialView(newViewContext, viewName).View;
				view.Render(newViewContext, newViewContext.Writer);
			}
		}
	}
}