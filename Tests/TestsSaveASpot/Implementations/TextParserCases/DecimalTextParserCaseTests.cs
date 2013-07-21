using NUnit.Framework;
using SaveASpot.Services.Implementations.TextParserCases;

namespace TestsSaveASpot.Implementations.TextParserCases
{
	[TestFixture]
	public sealed class DecimalTextParserCaseTests
	{
		public object[] TestCasesSource = new object[]
		{
			new object[]{"category", "category=123,1", true, "123,1"}, 
			new object[]{"category", "category=123,1;", true, "123,1"}, 
			new object[]{"category", "test;category=123,1;", true, "123,1"}, 
			new object[]{"category", ";category=123,2;", true, "123,2"}, 
			new object[]{"category", "category=123,3;", true, "123,3"}, 
			new object[]{"category", "category=123.3;", true, "123.3"}, 
			new object[]{"category", "category=123;", true, "123"}, 
			new object[]{"category", "catEgory=123;", true, "123"}, 
			new object[]{"category", "category=123s3;", true, "123"}, 

			new object[]{"category", "categoy=123.3;", false, string.Empty}, 
		};

		[Test, TestCaseSource("TestCasesSource")]
		public void Parse_should_return_value(string name, string input, bool expected, string expectedStatus)
		{
			//arrange
			var target = new DecimalTextParserCase(name);
			//act
			var actual = target.Parse(input);
			//assert
			Assert.AreEqual(expected, actual.IsSuccess);
			Assert.AreEqual(expectedStatus, actual.Status);
		}
	}
}
