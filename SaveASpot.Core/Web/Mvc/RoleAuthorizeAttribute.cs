using System;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public class RoleAuthorizeAttribute : Attribute
	{
		private readonly Type _roleType;
		public Type RoleType { get { return _roleType; } }

		public RoleAuthorizeAttribute(Type roleType)
		{
			if (!IsDerivedFromRole(roleType)) throw new ArgumentException("Argument 'roleType' should be derived from <Role>.");

			_roleType = roleType;
		}

		private bool IsDerivedFromRole(Type type)
		{
			return TypeHelper.IsDerivedFromType(type, typeof(Role));
		}

		#region sealed members
		public override sealed bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override sealed int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override sealed bool IsDefaultAttribute()
		{
			return base.IsDefaultAttribute();
		}

		public override sealed bool Match(object obj)
		{
			return base.Match(obj);
		}

		public override sealed string ToString()
		{
			return base.ToString();
		}

		public override sealed object TypeId
		{
			get
			{
				return base.TypeId;
			}
		}
		#endregion sealed members
	}
}
