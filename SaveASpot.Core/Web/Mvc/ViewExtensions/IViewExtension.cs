using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public interface IViewExtension
	{
		IEnumerable<IViewExtensionResult> CollectionExtensions();
	}

	public interface IRequiredArgumentViewExtension
	{
		IEnumerable<KeyValuePair<string, string>> Args { set; }
	}
}