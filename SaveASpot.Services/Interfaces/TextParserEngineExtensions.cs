using System;
using System.Linq.Expressions;

namespace SaveASpot.Services.Interfaces
{
	public static class TextParserEngineExtensions
	{
		public static TextParserBuilder BuildDecimal<T>(this ITextParserEngine source, Expression<Func<T, decimal>> expression, Action<decimal> action)
		{
			var result = new TextParserBuilder(source);
			result.Add(source.CreateParser(expression), DecimalAction(action));

			return result;
		}

		public static TextParserBuilder BuildDecimal<T>(this TextParserBuilder source, Expression<Func<T, decimal>> expression, Action<decimal> action)
		{
			source.Add(source.TextParserEngine.CreateParser(expression), DecimalAction(action));
			return source;
		}

		private static Action<string> DecimalAction(Action<decimal> action)
		{
			return val =>
							 {
								 decimal value;
								 if (decimal.TryParse(val, out value))
								 {
									 action(value);
								 }
							 };
		}
	}
}