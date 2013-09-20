using NUnit.Framework;
using SaveASpot.Core.Geocoding;

namespace TestsSaveASpot.Core.Geocoding
{
	[TestFixture]
	public sealed class PointTests
	{
		[Test]
		[TestCaseSource("TestCases")]
		public void PointInSquare_should_return_correct_value(Point source, Point leftBottom, Point rightTop, bool isInSquare)
		{
			//arrange
			//act
			var actual = Point.PointInSquare(source, leftBottom, rightTop);
			//assert
			Assert.AreEqual(isInSquare, actual);
		}

		public static object[] TestCases = new object[]
		{
			new object[]{ new Point{Longitude = 1, Latitude = 1}, new Point{Longitude = -1, Latitude = -1}, new Point{Longitude = 3, Latitude = 3}, true },

			new object[]{ new Point{Longitude = 1, Latitude = 1}, new Point{Longitude = 10, Latitude = 10}, new Point{Longitude = 3, Latitude = 3}, false }
		};
	}
}
