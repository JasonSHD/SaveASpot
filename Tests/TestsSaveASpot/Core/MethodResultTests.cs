using NUnit.Framework;
using SaveASpot.Core;

namespace TestsSaveASpot.Core
{
	[TestFixture]
	public sealed class MethodResultTests
	{
		public static object[] CtorTestCasesSource = new object[]
		{
			new object[]{true, new Status()},
			new object[]{false, new Status()},
		};

		[Test]
		[TestCaseSource("CtorTestCasesSource")]
		public void ctor_should_initialize_properties(bool isSuccess, Status expected)
		{
			//arrange
			//act
			var target = new MethodResult<Status>(isSuccess, expected);
			//assert
			Assert.AreEqual(isSuccess, target.IsSuccess);
			Assert.AreEqual(expected, target.Status);
		}

		public sealed class Status { }
	}
}
