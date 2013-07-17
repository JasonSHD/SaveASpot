using System;

namespace SaveASpot.Core.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class TabDescriptionsAttribute : Attribute
	{
		private readonly string _alias;
		private readonly string _title;

		public Type ControllerType { get; set; }
		public int IndexOfOrder { get; set; }
		public string Alias { get { return _alias; } }
		public string Title { get { return _title; } }

		public TabDescriptionsAttribute(string alias, string title)
		{
			_alias = alias;
			_title = title;
		}
	}
}