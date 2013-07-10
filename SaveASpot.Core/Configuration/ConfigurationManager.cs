namespace SaveASpot.Core.Configuration
{
	public sealed class ConfigurationManager : IConfigurationManager
	{
		public string GetSettings(string key)
		{
			return System.Configuration.ConfigurationManager.AppSettings[key];
		}

		public string GetConnectionString(string key)
		{
			return System.Configuration.ConfigurationManager.ConnectionStrings[key].ConnectionString;
		}
	}
}