using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SaveASpot.Core.Web.Mvc
{
	public interface ITabDescriptionControllerTypes
	{
		IEnumerable<Type> GetTypes();
	}

	public sealed class TabDescriptionControllerTypes : ITabDescriptionControllerTypes
	{
		private readonly Assembly _assembly;

		public TabDescriptionControllerTypes(Assembly assembly)
		{
			_assembly = assembly;
		}

		public IEnumerable<Type> GetTypes()
		{
			return _assembly.GetTypes().Where(e => TypeHelper.IsDerivedFromType(e, typeof(BaseController)));
		}
	}
}