using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class TabActionFilter : IActionFilter
	{
		private readonly ITabElementFilter _tabElementFilter;
		private readonly IRoleFactory _roleFactory;
		private readonly IControllerTypesFinder _controllerTypesFinder;
		private readonly ITextService _textService;
		private readonly IDictionary<Type, IList<TabElement>> _tabElements;
		private readonly object _locker = new object();

		public TabActionFilter(ITabElementFilter tabElementFilter, IRoleFactory roleFactory, IControllerTypesFinder controllerTypesFinder, ITextService textService)
		{
			_tabElementFilter = tabElementFilter;
			_roleFactory = roleFactory;
			_controllerTypesFinder = controllerTypesFinder;
			_textService = textService;
			_tabElements = new Dictionary<Type, IList<TabElement>>();
		}

		private IEnumerable<KeyValuePair<Type, IEnumerable<TabElement>>> GetTabsInSystem()
		{
			if (_tabElements.Values.Count == 0)
			{
				lock (_locker)
				{
					if (_tabElements.Values.Count == 0)
					{
						FindTabsInSystem();
					}
				}
			}

			return _tabElements.Select(e => new KeyValuePair<Type, IEnumerable<TabElement>>(e.Key, e.Value));
		}

		private void FindTabsInSystem()
		{
			foreach (Type type in _controllerTypesFinder.GetTypes())
			{
				if (!TypeHelper.IsDerivedFromType(type, typeof(BaseController))) continue;

				var tabAttributes = type.GetCustomAttributes(false).OfType<TabAttribute>().ToList();
				var roles = type.GetCustomAttributes(false).OfType<RoleAuthorizeAttribute>().Select(e => _roleFactory.Convert(e.RoleType)).ToList();

				foreach (TabAttribute tabAttribute in tabAttributes)
				{
					IList<TabElement> tabElements;
					if (_tabElements.ContainsKey(tabAttribute.GetType()))
					{
						tabElements = _tabElements[tabAttribute.GetType()];
					}
					else
					{
						tabElements = new List<TabElement>();
						_tabElements.Add(tabAttribute.GetType(), tabElements);
					}

					tabElements.Add(new TabElement
					{
						ActionName = "Index",
						Alias = tabAttribute.Alias,
						Area = tabAttribute.Area,
						ControllerType = type,
						IndexOfOrder = tabAttribute.IndexOfOrder,
						Roles = roles,
						Title = _textService.ResolveTest(tabAttribute.Title)
					});
				}
			}
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (TypeHelper.IsDerivedFromType(filterContext.Controller.GetType(), typeof(TabController)))
			{
				var tabsInSystem = GetTabsInSystem();
				var result = new Dictionary<Type, IEnumerable<TabElement>>();

				foreach (KeyValuePair<Type, IEnumerable<TabElement>> keyValuePair in tabsInSystem)
				{
					var filtredElements = _tabElementFilter.Filter(keyValuePair.Value);
					result.Add(keyValuePair.Key, filtredElements);
				}

				TabElement.SetDescriptions(result, filterContext.Controller.ViewBag);
			}
		}
	}
}