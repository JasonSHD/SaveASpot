using System;

namespace SaveASpot.Core.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class TabDataAttribute : Attribute
	{
		public TabDataAttribute()
		{
			MasterPath = string.Empty;
			RequestHeader = string.Empty;
			AjaxMaterPath = string.Empty;
		}

		public string MasterPath { get; set; }
		public string RequestHeader { get; set; }
		public string AjaxMaterPath { get; set; }
	}
}