namespace SaveASpot.Core
{
	public interface IStrategy<in TArg, out TResult>
	{
		IMethodResult<TResult> Execute(TArg arg);
	}
}