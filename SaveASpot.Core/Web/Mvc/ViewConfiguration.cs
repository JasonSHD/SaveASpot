using SaveASpot.Core.Security;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewConfiguration
	{
		private readonly bool _useMinScripts;
		private readonly User _user;
		public bool UseMinimizedScripts { get { return _useMinScripts; } }
		public User User { get { return _user; } }

		public ViewConfiguration(bool useMinScripts, User user)
		{
			_useMinScripts = useMinScripts;
			_user = user;
		}
	}
}