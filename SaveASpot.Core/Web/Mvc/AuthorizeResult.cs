namespace SaveASpot.Core.Web.Mvc
{
	public sealed class AuthorizeResult
	{
		private readonly bool _isAllow;
		public bool IsAllow { get { return _isAllow; } }

		public AuthorizeResult(bool isAllow)
		{
			_isAllow = isAllow;
		}
	}
}