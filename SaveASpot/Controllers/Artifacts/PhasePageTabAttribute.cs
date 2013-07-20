using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	[TabData(MasterPath = SiteConstants.Layouts.ParcelsAndSpotsLayout)]
	public sealed class PhasePageTabAttribute : TabAttribute
	{
		public PhasePageTabAttribute()
			: base(typeof(PhasePageDefaultTabActionAttribute))
		{
		}
	}
}