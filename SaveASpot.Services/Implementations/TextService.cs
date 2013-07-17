using System.Reflection;
using System.Resources;
using SaveASpot.Core;
using SaveASpot.Core.Logging;

namespace SaveASpot.Services.Implementations
{
	public sealed class TextService : ITextService
	{
		private readonly ILogger _logger;
		private ResourceManager _resourceManager;
		private ResourceManager ResourceManager
		{
			get
			{
				return _resourceManager ??
							 (_resourceManager =
								new ResourceManager("SaveASpot.Services.Properties.Resources", Assembly.GetExecutingAssembly()));
			}
		}

		public TextService(ILogger logger)
		{
			_logger = logger;
		}

		public string ResolveTest(string key)
		{
			if (string.IsNullOrWhiteSpace(key)) return string.Empty;

			var result = ResourceManager.GetString(key);
			if (string.IsNullOrWhiteSpace(result))
			{
				_logger.Info("Model metadata request to resolve string <{0}> but it not found.", key);
			}

			return result;
		}
	}
}
