namespace SaveASpot.Core.Security
{
	public interface ICurrentUser
	{
		User User { get; }
	}

	public interface IAnonymUser
	{
		User User { get; }
	}
}
