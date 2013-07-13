using System;
using SaveASpot.Core.Configuration;

namespace SaveASpot.Areas.Setup.Controllers.Artifacts
{
	public sealed class SetupConfiguration : ISetupConfiguration
	{
		private readonly IConfigurationManager _configurationManager;

		public SetupConfiguration(IConfigurationManager configurationManager)
		{
			_configurationManager = configurationManager;
		}

		public bool IsEnabled
		{
			get
			{
				var setuppEnabledValue = _configurationManager.GetSettings("SetupEnabled");

				return !string.IsNullOrWhiteSpace(setuppEnabledValue) && setuppEnabledValue.Equals("true", StringComparison.InvariantCultureIgnoreCase);
			}
		}
	}
}