using System;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.Extensions
{
	public sealed class ContainerForElement : IDisposable
	{
		private readonly HtmlHelper _htmlHelper;
		private readonly TagBuilder _tagBuilder;

		public const string ForAllContainerValue = "*";

		public ContainerForElement(HtmlHelper htmlHelper, string htmlElement, TabElement element, bool forAll, object htmlAttributes, string typeName)
		{
			if (element == null && !forAll) throw new ArgumentNullException("element");

			_htmlHelper = htmlHelper;
			_tagBuilder = new TagBuilder(htmlElement);
			_tagBuilder.MergeAttribute("data-ajaxform-container-" + (forAll ? typeName : element.Alias), string.Empty);
			_tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

			_htmlHelper.ViewContext.Writer.Write(_tagBuilder.ToString(TagRenderMode.StartTag));
		}

		public void Dispose()
		{
			_htmlHelper.ViewContext.Writer.Write(_tagBuilder.ToString(TagRenderMode.EndTag));
		}
	}
}