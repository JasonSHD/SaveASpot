namespace SaveASpot.Repositories.Implementations
{
	public interface IMongoDBConfiguration
	{
		string ConnectionString { get; }
		string DatabaseName { get; }
	}
}