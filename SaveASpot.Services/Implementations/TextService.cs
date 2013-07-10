using System.Reflection;
using System.Resources;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class TextService : ITextService
	{
		public string ResolveTest(string key)
		{
			if (string.IsNullOrWhiteSpace(key)) return string.Empty;

			var resources = new ResourceManager("SaveASpot.Services.Properties.Resources", Assembly.GetExecutingAssembly());
			return resources.GetString(key);
		}
	}
}
