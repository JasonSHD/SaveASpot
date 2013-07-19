using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.Extensions
{
	public static class TabElementExtensions
	{
		public static IEnumerable<TabElement> For<TElement>(this HtmlHelper source) where TElement : TabAttribute
		{
			var elements = (IDictionary<Type, IEnumerable<TabElement>>)TabElement.GetDescriptions(source.ViewContext.Controller.ViewBag);
			if (elements == null) throw new KeyNotFoundException();

			return elements.First(e => e.Key == typeof(TElement)).Value;
		}

		public static MvcHtmlString ButtonForElement<TElement>(this HtmlHelper source, string htmlElement, string alias,
																																			 object htmlAttributes) where TElement : TabAttribute
		{
			var htmlBuilder = new TagBuilder(htmlElement);
			var element = source.For<TElement>().First(e => e.Alias == alias);
			var urlHelper = new UrlHelper(source.ViewContext.RequestContext);

			htmlBuilder.SetInnerText(element.Title);
			htmlBuilder.MergeAttribute("data-ajaxform", typeof(TElement).Name);
			htmlBuilder.MergeAttribute("data-ajaxform-alias", alias);
			htmlBuilder.MergeAttribute("data-ajaxform-url", urlHelper.Action(element.ActionName, element.ControllerName, new { area = element.Area }));

			htmlBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), true);

			return new MvcHtmlString(htmlBuilder.ToString());
		}

		public static ContainerForElement BeginContainerForElement<TElement>(this HtmlHelper source, string htmlElement, string alias = ContainerForElement.ForAllContainerValue, bool forAll = false, object htmlAttributes = null)
			where TElement : TabAttribute
		{
			var element = source.For<TElement>().FirstOrDefault(e => e.Alias == alias);
			var containerForElement = new ContainerForElement(source, htmlElement, element, forAll, htmlAttributes, typeof(TElement).Name);

			return containerForElement;
		}
	}
}
