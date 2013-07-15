using System;

namespace SaveASpot.Core
{
	public static class TypeHelper
	{
		public static bool IsDerivedFromType(Type type, Type baseType)
		{
			if (type == baseType) return true;
			if (type == null) return false;

			return IsDerivedFromType(type.BaseType, baseType);
		}
	}
}