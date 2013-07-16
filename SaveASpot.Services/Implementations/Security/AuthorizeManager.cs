using System;
using System.Collections.Generic;
using System.Linq;
using SaveASpot.Core.Security;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Services.Implementations.Security
{
	public sealed class AuthorizeManager : IAuthorizeManager
	{
		private readonly ICurrentUser _currentUser;
		private readonly IRoleFactory _roleFactory;

		public AuthorizeManager(ICurrentUser currentUser, IRoleFactory roleFactory)
		{
			_currentUser = currentUser;
			_roleFactory = roleFactory;
		}

		public AuthorizeResult AllowAccess(IEnumerable<Type> roleTypes)
		{
			var roleTypesArray = roleTypes as Type[] ?? roleTypes.ToArray();
			if (!roleTypesArray.Any()) return new AuthorizeResult(true);
			var roles = roleTypesArray.Select(e => _roleFactory.Convert(e));

			return new AuthorizeResult(roles.Any(e => _currentUser.User.Roles.Any(role => role == e)));
		}
	}
}
