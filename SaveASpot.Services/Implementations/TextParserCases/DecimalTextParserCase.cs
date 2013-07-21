using System.Text.RegularExpressions;
using SaveASpot.Core;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations.TextParserCases
{
	public sealed class DecimalTextParserCase : ITextParserCase
	{
		private readonly string _name;

		public DecimalTextParserCase(string name)
		{
			_name = name;
		}

		public IMethodResult<string> Parse(string input)
		{
			var regex = new Regex("(?<=" + _name + "=)[0-9.,]*(?<=[0-9,.])", RegexOptions.IgnoreCase);
			var result = regex.Match(input);

			return new MethodResult<string>(result.Success, result.Success ? result.Value : string.Empty);
		}
	}
}