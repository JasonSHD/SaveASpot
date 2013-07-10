namespace SaveASpot.Core.Configuration
{
	public interface IConfigurationManager
	{
		string GetSettings(string key);
		string GetConnectionString(string key);
	}
}
