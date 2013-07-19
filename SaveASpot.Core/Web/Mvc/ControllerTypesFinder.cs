using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ControllerTypesFinder : IControllerTypesFinder
	{
		private readonly Assembly _assembly;

		public ControllerTypesFinder(Assembly assembly)
		{
			_assembly = assembly;
		}

		public IEnumerable<Type> GetTypes()
		{
			return _assembly.GetTypes().Where(e => TypeHelper.IsDerivedFromType(e, typeof(BaseController)));
		}
	}
}