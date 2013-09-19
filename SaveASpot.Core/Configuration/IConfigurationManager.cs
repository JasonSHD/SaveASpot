using System;

namespace SaveASpot.Core.Configuration
{
	public interface IConfigurationManager
	{
		string GetSettings(string key);
		string GetConnectionString(string key);
	}

	public static class ConfigurationManagerExtensions
	{
		public static IMethodResult<int> GetInt32Settings(this IConfigurationManager source, string key, int defaultValue = default(Int32))
		{
			var value = source.GetSettings(key);
			Int32 result;
			if (Int32.TryParse(value, out result))
			{
				return new MethodResult<int>(true, result);
			}

			return new MethodResult<int>(false, defaultValue);
		}
	}
}
