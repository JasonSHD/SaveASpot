using System;
using System.Linq;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TabDescriptionsAttribute : Attribute
	{
		private readonly string _alias;
		private readonly Type[] _roles;
		private readonly string _title;

		public Type ControllerType { get; set; }
		public string Alias { get { return _alias; } }
		public Type[] Roles { get { return _roles; } }
		public string Title { get { return _title; } }

		public TabDescriptionsAttribute(string alias, Type[] roles, string title)
		{
			if (roles.Any(r => !TypeHelper.IsDerivedFromType(r, typeof(Role)))) throw new ArgumentException("All items in <roles> should be derived from <Role>.", "roles");

			_alias = alias;
			_roles = roles;
			_title = title;
		}
	}


}