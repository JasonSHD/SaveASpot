using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class TabElement
	{
		public Type ControllerType { get; set; }
		public string ControllerName { get { return ControllerType.Name.Replace("Controller", string.Empty); } }
		public string ActionName { get; set; }
		public string Area { get; set; }

		public string Title { get; set; }
		public string Alias { get; set; }
		public int IndexOfOrder { get; set; }
		public IEnumerable<Role> Roles { get; set; }

		public static void SetDescriptions(IDictionary<Type, IEnumerable<TabElement>> tabDescriptions, dynamic viewBag)
		{
			viewBag.TabElementsDescription = tabDescriptions;
		}

		public static IDictionary<Type, IEnumerable<TabElement>> GetDescriptions(dynamic viewBag)
		{
			return (IDictionary<Type, IEnumerable<TabElement>>)viewBag.TabElementsDescription;
		}
	}

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

	public abstract class DefaultTabActionAttribute : Attribute { }

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class TabDataAttribute : Attribute
	{
		public TabDataAttribute()
		{
			MasterPath = string.Empty;
			RequestHeader = string.Empty;
		}

		public string MasterPath { get; set; }
		public string RequestHeader { get; set; }
	}

	public abstract class TabController : BaseController
	{
		protected virtual ViewResult TabView(object model)
		{
			//var actionName = this.RouteData.Values["action"];

			//GetType().GetMethod()

			//var specifiedMasterName = Request.Headers.AllKeys.All(e => e != SiteConstants.AdminTabControlHeader);

			var view = View(null, null, model);
			
			return view;
		}
	}
}
