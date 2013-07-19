using System;
using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc
{
	public interface IControllerTypesFinder
	{
		IEnumerable<Type> GetTypes();
	}
}