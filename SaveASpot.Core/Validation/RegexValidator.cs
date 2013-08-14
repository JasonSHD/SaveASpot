using System.Text.RegularExpressions;

namespace SaveASpot.Core.Validation
{
	public sealed class RegexValidator : BaseValidator<string>
	{
		private readonly Regex _regex;

		public RegexValidator(string regex)
			: base("InvalidString")
		{
			_regex = new Regex(regex, RegexOptions.Compiled);
		}

		protected override bool IsValid(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return true;
			}

			return _regex.IsMatch(input);
		}
	}
}