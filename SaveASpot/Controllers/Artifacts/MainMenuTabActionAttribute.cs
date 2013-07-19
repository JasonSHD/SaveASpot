using System;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers.Artifacts
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public sealed class MainMenuTabActionAttribute : DefaultTabActionAttribute { }
}