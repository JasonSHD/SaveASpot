using NUnit.Framework;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class PolygonTests
	{
		[Test]
		public void PointInPolygon_innerPoint_containedWithinPolygon()
		{
			//arrange
			var vertices = new[]
				               {  
					               new Point(){ Longitude = 1, Latitude = 3 },  
					               new Point(){ Longitude = 1, Latitude = 1 },  
					               new Point(){ Longitude = 4, Latitude = 1 },  
					               new Point(){ Longitude = 4, Latitude = 3 }  
				               };
			var p = new Polygon(vertices);
			//act
			var actual = p.PointInPolygon(new Point() { Longitude = 2, Latitude = 2 });
			//arrage
			Assert.AreEqual(true, actual);
		}

		[Test]
		public void PointInPolygon_outerPoint_notContainedWithinPolygon()
		{
			//arrange
			var vertices = new[]
				               {  
					               new Point() { Longitude = 1, Latitude = 3 },  
					               new Point() { Longitude = 1, Latitude = 1 },  
					               new Point() { Longitude = 4, Latitude = 1 },  
					               new Point() { Longitude = 4, Latitude = 3 }
				               };

			var p = new Polygon(vertices);
			//act
			var actual = p.PointInPolygon(new Point() { Longitude = 5, Latitude = 3 });
			//assert
			Assert.AreEqual(false, actual);
		}
	}
}