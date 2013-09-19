namespace SaveASpot.Core.Strategies
{
	public sealed class StrategyResult<TStatus> : IMethodResult<TStatus>, IStrategyResult
	{
		private readonly bool _isSuccess;
		private readonly bool _isBreak;
		private readonly TStatus _status;

		public bool IsSuccess { get { return _isSuccess; } }

		public TStatus Status { get { return _status; } }

		public bool IsBreak { get { return _isBreak; } }

		public StrategyResult(bool isSuccess, bool isBreak, TStatus status)
		{
			_isSuccess = isSuccess;
			_isBreak = isBreak;
			_status = status;
		}
	}
}