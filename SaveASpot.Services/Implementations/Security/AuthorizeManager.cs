using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces.Security;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class AuthorizeManager : IAuthorizeManager
	{
		private readonly ICurrentUser _currentUser;
		private readonly IRoleHarvester _roleHarvester;

		public AuthorizeManager(ICurrentUser currentUser, IRoleHarvester roleHarvester)
		{
			_currentUser = currentUser;
			_roleHarvester = roleHarvester;
		}

		public AuthorizeResult AllowAccess(IEnumerable<Type> roleTypes)
		{
			var roleTypesArray = roleTypes as Type[] ?? roleTypes.ToArray();
			if (!roleTypesArray.Any()) return new AuthorizeResult(true);
			var roles = roleTypesArray.Select(e => _roleHarvester.Convert(e));

			return new AuthorizeResult(roles.Any(e => _currentUser.User.Roles.Any(role => role == e)));
		}
	}
}
