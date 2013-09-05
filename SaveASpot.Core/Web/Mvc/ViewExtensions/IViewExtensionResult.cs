namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public interface IViewExtensionResult
	{
		string ViewName { get; }
		object Model { get; }
		IElementIdentity RenderViewExtensionIdentity { get; }
	}
}