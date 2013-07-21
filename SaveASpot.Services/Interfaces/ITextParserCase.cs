using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ITextParserCase
	{
		IMethodResult<string> Parse(string input);
	}
}