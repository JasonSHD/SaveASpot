using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class TabDescriptionActionFilter : IActionFilter
	{
		private readonly IRoleFactory _roleFactory;
		private readonly ITabDescriptionControllerTypes _tabDescriptionControllerTypes;
		private readonly ITabDescriptionFilter _tabDescriptionFilter;
		private readonly ITextService _textService;
		private readonly IList<TabDescription> _tabDescriptions = new List<TabDescription>();

		public TabDescriptionActionFilter(IRoleFactory roleFactory, ITabDescriptionControllerTypes tabDescriptionControllerTypes, ITabDescriptionFilter tabDescriptionFilter, ITextService textService)
		{
			_roleFactory = roleFactory;
			_tabDescriptionControllerTypes = tabDescriptionControllerTypes;
			_tabDescriptionFilter = tabDescriptionFilter;
			_textService = textService;
		}

		private IEnumerable<TabDescription> GetTabDescriptions()
		{
			if (_tabDescriptions.Count == 0)
			{
				foreach (var type in _tabDescriptionControllerTypes.GetTypes())
				{
					if (TypeHelper.IsDerivedFromType(type, typeof(BaseController)))
					{
						var tabDescriptionAttributes =
							type.GetCustomAttributes(false).Where(e => e is TabDescriptionsAttribute).Cast<TabDescriptionsAttribute>();
						var roleAuthorizeAttributes =
							type.GetCustomAttributes(false).Where(e => TypeHelper.IsDerivedFromType(e.GetType(), typeof(RoleAuthorizeAttribute))).Cast<RoleAuthorizeAttribute>().Select(e => _roleFactory.Convert(e.RoleType)).ToList();

						foreach (var tabDescriptionsAttribute in tabDescriptionAttributes)
						{
							if (tabDescriptionsAttribute.ControllerType == null)
							{
								tabDescriptionsAttribute.ControllerType = type;
							}
							_tabDescriptions.Add(new TabDescription
																		 {
																			 Alias = tabDescriptionsAttribute.Alias,
																			 ControllerType = tabDescriptionsAttribute.ControllerType,
																			 Roles = roleAuthorizeAttributes.ToList(),
																			 Title = _textService.ResolveTest(tabDescriptionsAttribute.Title)
																		 });
						}
					}
				}
			}

			return _tabDescriptions;
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
		}

		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (TypeHelper.IsDerivedFromType(filterContext.Controller.GetType(), typeof(BaseController)))
			{
				TabDescription.SetDescriptions(_tabDescriptionFilter.Filter(GetTabDescriptions()), filterContext.Controller.ViewBag);
			}
		}
	}
}