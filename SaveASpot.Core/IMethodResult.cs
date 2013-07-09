namespace SaveASpot.Core
{
	public interface IMethodResult<out T> : IMethodResult
	{
		T Status { get; }
	}

	public interface IMethodResult
	{
		bool IsSuccess { get; }
	}
}
