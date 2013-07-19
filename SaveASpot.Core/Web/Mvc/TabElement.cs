using System;
using System.Collections.Generic;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class TabElement
	{
		public Type ControllerType { get; set; }
		public string ControllerName { get { return ControllerType.Name.Replace("Controller", string.Empty); } }
		public string ActionName { get; set; }
		public string Area { get; set; }

		public string Title { get; set; }
		public string Alias { get; set; }
		public int IndexOfOrder { get; set; }
		public IEnumerable<Role> Roles { get; set; }

		public static void SetDescriptions(IDictionary<Type, IEnumerable<TabElement>> tabDescriptions, dynamic viewBag)
		{
			viewBag.TabElementsDescription = tabDescriptions;
		}

		public static IDictionary<Type, IEnumerable<TabElement>> GetDescriptions(dynamic viewBag)
		{
			return (IDictionary<Type, IEnumerable<TabElement>>)viewBag.TabElementsDescription;
		}
	}
}
