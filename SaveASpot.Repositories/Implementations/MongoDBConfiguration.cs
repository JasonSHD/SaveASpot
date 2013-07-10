using SaveASpot.Core.Configuration;

namespace SaveASpot.Repositories.Implementations
{
	public sealed class MongoDBConfiguration : IMongoDBConfiguration
	{
		private readonly IConfigurationManager _configurationManager;

		public MongoDBConfiguration(IConfigurationManager configurationManager)
		{
			_configurationManager = configurationManager;
		}
		public string ConnectionString { get { return _configurationManager.GetConnectionString("MongoDBDefaultConnectionString"); } }
		public string DatabaseName { get { return _configurationManager.GetSettings("MongodDBDefaultDatabaseName"); } }
	}
}