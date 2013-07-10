namespace SaveASpot.Core.Security
{
	public interface IPasswordHash
	{
		string GetHash(string password, string username);
	}
}