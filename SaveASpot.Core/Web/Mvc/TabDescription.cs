using System;
using System.Collections.Generic;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class TabDescription
	{
		public Type ControllerType { get; set; }
		public string ControllerName { get { return ControllerType.Name.Replace("Controller", string.Empty); } }
		public string ActionName { get { return "Index"; } }
		public string Title { get; set; }
		public string Alias { get; set; }
		public IEnumerable<Role> Roles { get; set; }

		public static void SetDescriptions(IEnumerable<TabDescription> tabDescriptions, dynamic viewBag)
		{
			viewBag.AdminTabDescriptions = tabDescriptions;
		}

		public static IEnumerable<TabDescription> GetDescriptions(dynamic viewBag)
		{
			return viewBag.AdminTabDescriptions;
		}
	}
}