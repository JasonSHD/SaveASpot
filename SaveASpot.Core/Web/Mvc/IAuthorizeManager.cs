using System;
using System.Collections.Generic;

namespace SaveASpot.Core.Web.Mvc
{
	public interface IAuthorizeManager
	{
		AuthorizeResult AllowAccess(IEnumerable<Type> roles);
	}
}