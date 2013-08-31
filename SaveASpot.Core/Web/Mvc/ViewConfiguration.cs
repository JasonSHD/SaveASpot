using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewConfiguration
	{
		private readonly bool _useMinScripts;
		private readonly User _user;
		private readonly User _anonymUser;
		public bool UseMinimizedScripts { get { return _useMinScripts; } }
		public User User { get { return _user; } }
		public User Anonym { get { return _anonymUser; } }

		public ViewConfiguration(bool useMinScripts, User user, User anonymUser)
		{
			_useMinScripts = useMinScripts;
			_user = user;
			_anonymUser = anonymUser;
		}
	}
}