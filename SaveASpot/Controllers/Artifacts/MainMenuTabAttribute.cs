using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	[TabData(MasterPath = SiteConstants.Layouts.AdminTabMasterName)]
	public sealed class MainMenuTabAttribute : TabAttribute
	{
		public MainMenuTabAttribute()
			: base(typeof(MainMenuTabActionAttribute))
		{
		}
	}
}