namespace SaveASpot.Core
{
	public sealed class MethodResult<T> : IMethodResult<T>
	{
		private readonly bool _isSuccess;
		private readonly T _status;

		public MethodResult(bool isSuccess, T status)
		{
			_isSuccess = isSuccess;
			_status = status;
		}

		public bool IsSuccess { get { return _isSuccess; } }
		public T Status { get { return _status; } }
	}
}