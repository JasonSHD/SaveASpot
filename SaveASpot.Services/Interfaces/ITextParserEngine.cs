using System;
using System.Linq.Expressions;

namespace SaveASpot.Services.Interfaces
{
	public interface ITextParserEngine
	{
		ITextParserCase CreateParser<T, TResult>(Expression<Func<T, TResult>> expression);
	}
}