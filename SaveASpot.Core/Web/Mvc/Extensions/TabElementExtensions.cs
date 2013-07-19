using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc.Extensions
{
	public static class TabElementExtensions
	{
		public static IEnumerable<TabElement> For<TTabElementAttribute>(this HtmlHelper source) where TTabElementAttribute : TabAttribute
		{
			var elements = (IDictionary<Type, IEnumerable<TabElement>>)TabElement.GetDescriptions(source.ViewContext.Controller.ViewBag);
			if (elements == null) throw new KeyNotFoundException();

			return elements.First(e => e.Key == typeof(TTabElementAttribute)).Value;
		}
	}
}
