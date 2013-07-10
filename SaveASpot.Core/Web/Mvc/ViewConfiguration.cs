namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ViewConfiguration
	{
		private readonly bool _useMinScripts;
		public bool UseMinimizedScripts { get { return _useMinScripts; } }

		public ViewConfiguration(bool useMinScripts)
		{
			_useMinScripts = useMinScripts;
		}
	}
}