using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class PhasePageDefaultTabActionAttribute : DefaultTabActionAttribute { }
}