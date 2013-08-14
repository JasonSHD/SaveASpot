using System.Collections.Generic;

namespace SaveASpot.Core
{
	public interface IValidationResult
	{
		bool IsValid { get; }
		string Message { get; }
		IEnumerable<KeyValuePair<string, string>> Data { get; }
	}
}