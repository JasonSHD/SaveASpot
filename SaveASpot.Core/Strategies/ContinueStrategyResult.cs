namespace SaveASpot.Core.Strategies
{
	public sealed class ContinueStrategyResult<TStatus> : IMethodResult<TStatus>, IStrategyResult
	{
		private readonly TStatus _status;
		private readonly bool _isSuccess;

		public ContinueStrategyResult(TStatus status, bool isSuccess = false)
		{
			_status = status;
			_isSuccess = isSuccess;
		}

		public bool IsSuccess { get { return _isSuccess; } }
		public TStatus Status { get { return _status; } }
		public bool IsBreak { get { return false; } }
	}
}