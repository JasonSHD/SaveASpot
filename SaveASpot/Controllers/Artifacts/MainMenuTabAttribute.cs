using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
<<<<<<< HEAD
	[TabData(MasterPath = SiteConstants.Layouts.MainMenuTabs)]
=======
	[TabData(MasterPath = SiteConstants.Layouts.AdminTabMasterName)]
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
	public sealed class MainMenuTabAttribute : TabAttribute
	{
		public MainMenuTabAttribute()
			: base(typeof(MainMenuTabActionAttribute))
		{
		}
	}
}