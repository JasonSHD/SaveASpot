using System;

namespace SaveASpot.Core.Configuration
{
	public sealed class ApplicationConfiguration : IApplicationConfiguration
	{
		private readonly ApplicationMode _applicationMode;
		public ApplicationMode Mode { get { return _applicationMode; } }

		public ApplicationConfiguration(IConfigurationManager configurationManager)
		{
			ApplicationMode mode;
			if (!Enum.TryParse(configurationManager.GetSettings("ApplicationMode"), out mode))
			{
				mode = ApplicationMode.Release;
			}

			_applicationMode = mode;
		}
	}
}