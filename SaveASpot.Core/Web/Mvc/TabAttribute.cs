using System;

namespace SaveASpot.Core.Web.Mvc
{
	public abstract class TabAttribute : Attribute
	{
		private readonly Type _defaultTabActionAttributeType;

		protected TabAttribute(Type defaultTabActionAttributeType)
		{
			if (!TypeHelper.IsDerivedFromType(defaultTabActionAttributeType, typeof(DefaultTabActionAttribute))) throw new ArgumentException();

			_defaultTabActionAttributeType = defaultTabActionAttributeType;
		}

		public string Area { get; set; }
		public string Title { get; set; }
		public string Alias { get; set; }
		public int IndexOfOrder { get; set; }
		public Type DefaultTabActionAttributeType { get { return _defaultTabActionAttributeType; } }

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