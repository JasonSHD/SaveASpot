using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class RoleTabElementFilter : ITabElementFilter
	{
		private readonly IAuthorizeManager _authorizeManager;

		public RoleTabElementFilter(IAuthorizeManager authorizeManager)
		{
			_authorizeManager = authorizeManager;
		}

		public IEnumerable<TabElement> Filter(IEnumerable<TabElement> filter)
		{
			return filter.Where(e => _authorizeManager.AllowAccess(e.Roles.Select(r => r.GetType())).IsAllow).ToList();
		}
	}
}