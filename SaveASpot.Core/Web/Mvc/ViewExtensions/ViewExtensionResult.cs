namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public sealed class ViewExtensionResult : IViewExtensionResult
	{
		private readonly string _viewName;
		private readonly object _model;
		private readonly IElementIdentity _renderViewExtensionIdentity;

		public string ViewName { get { return _viewName; } }
		public object Model { get { return _model; } }
		public IElementIdentity RenderViewExtensionIdentity { get { return _renderViewExtensionIdentity; } }

		public ViewExtensionResult(string viewName, object model, IElementIdentity renderViewExtensionIdentity)
		{
			_viewName = viewName;
			_model = model;
			_renderViewExtensionIdentity = renderViewExtensionIdentity;
		}
	}
}