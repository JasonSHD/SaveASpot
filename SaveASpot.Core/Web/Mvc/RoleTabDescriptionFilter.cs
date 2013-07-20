using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class RoleTabDescriptionFilter : ITabDescriptionFilter
	{
		private readonly IAuthorizeManager _authorizeManager;

		public RoleTabDescriptionFilter(IAuthorizeManager authorizeManager)
		{
			_authorizeManager = authorizeManager;
		}

		public IEnumerable<TabDescription> Filter(IEnumerable<TabDescription> tabDescriptions)
		{
			return tabDescriptions.Where(tabDescription => _authorizeManager.AllowAccess(tabDescription.Roles.Select(e => e.GetType())).IsAllow).ToList();
		}
	}
}