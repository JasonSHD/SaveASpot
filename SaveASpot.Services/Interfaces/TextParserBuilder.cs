using System;
using System.Collections.Generic;

namespace SaveASpot.Services.Interfaces
{
	public sealed class TextParserBuilder
	{
		private readonly ITextParserEngine _textParserEngine;
		public ITextParserEngine TextParserEngine { get { return _textParserEngine; } }
		private readonly IList<Tuple<ITextParserCase, Action<string>>> _textParserCases;

		public TextParserBuilder(ITextParserEngine textParserEngine)
		{
			_textParserEngine = textParserEngine;
			_textParserCases = new List<Tuple<ITextParserCase, Action<string>>>();
		}

		public void Add(ITextParserCase textParserCase, Action<string> action)
		{
			_textParserCases.Add(new Tuple<ITextParserCase, Action<string>>(textParserCase, action));
		}

		public void Build(string filter)
		{
			foreach (var textParserCase in _textParserCases)
			{
				var result = textParserCase.Item1.Parse(filter);
				if (result.IsSuccess)
				{
					textParserCase.Item2(result.Status);
				}
			}
		}
	}
}