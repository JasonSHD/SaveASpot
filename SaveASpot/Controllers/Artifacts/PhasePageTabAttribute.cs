using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
<<<<<<< HEAD
	[TabData(MasterPath = SiteConstants.Layouts.ParcelsAndSpotsLayout)]
=======
>>>>>>> 69c9ebf3750dc4ebe298d09f86679f1667ba5937
	public sealed class PhasePageTabAttribute : TabAttribute
	{
		public PhasePageTabAttribute()
			: base(typeof(PhasePageDefaultTabActionAttribute))
		{
		}
	}
}