using System;
using System.Linq.Expressions;
using SaveASpot.Services.Implementations.TextParserCases;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class TextParserEngine : ITextParserEngine
	{
		public ITextParserCase CreateParser<T, TResult>(Expression<Func<T, TResult>> expression)
		{
			return new DecimalTextParserCase(((MemberExpression)expression.Body).Member.Name);
		}
	}
}