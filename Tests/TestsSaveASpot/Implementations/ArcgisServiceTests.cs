using System.IO;
using NSubstitute;
using NUnit.Framework;
using SaveASpot.Core.Logging;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class ArcgisServiceTests
	{
		private ArcgisService ArcgisService { get; set; }
		private IParcelRepository ParcelRepository { get; set; }
		private IPhaseRepository PhaseRepository { get; set; }
		private ISpotRepository SpotRepository { get; set; }
        private IParcelQueryable _parcelQueryable;
        private ILogger _logger;

		[SetUp]
		public void SetUp()
		{
			ParcelRepository = Substitute.For<IParcelRepository>();
			PhaseRepository = Substitute.For<IPhaseRepository>();
			SpotRepository = Substitute.For<ISpotRepository>();
		    _parcelQueryable = Substitute.For<IParcelQueryable>();
            _logger = Substitute.For<ILogger>();

            ArcgisService = new ArcgisService(ParcelRepository, PhaseRepository, SpotRepository, _parcelQueryable, _logger);
		}

		private static string ParcelsResource1 { 
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase1.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			} 
		}
		private static string ParcelsResource2
		{
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase3.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			}
		}
		private static string ParcelsResource3
		{
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase4.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			}
		}

		public static object[] AddParcelsForParcels_TestCaseSource = new object[]
		{
			new object[]{ParcelsResource1, 4},
			new object[]{ParcelsResource2, 6},
			new object[]{ParcelsResource3, 8},
		};

		[Test, TestCaseSource("AddParcelsForParcels_TestCaseSource")]
		public void AddParcels_should_add_parcels(string input, int count)
		{
			//arrange
			//act
			ArcgisService.AddParcels(input);
			//assert
			ParcelRepository.Received(count).AddParcel(Arg.Any<Parcel>());
		}

		public static object[] AddParcelsForPhases_TestCaseSource = new object[]
		{
			new object[]{ParcelsResource1, 4},
			new object[]{ParcelsResource2, 6},
			new object[]{ParcelsResource3, 8},
		};

		[Test, TestCaseSource("AddParcelsForPhases_TestCaseSource")]
		public void AddParcels_should_add_phases(string input, int count)
		{
			//arrange
			//act
			ArcgisService.AddParcels(input);
			//assert
			PhaseRepository.Received(count).AddPhase(Arg.Any<Phase>());
		}

		private static string SpotsResource1
		{
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase1_Grid.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			}
		}
		private static string SpotsResource2
		{
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase3_Grid.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			}
		}
		private static string SpotsResource3
		{
			get
			{
				using (var r = new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase4_Grid.json"))
				{
					var json = r.ReadToEnd();
					return json;
				}
			}
		}

		public static object[] AddSpots_TestCaseSource = new object[]
		{
			new object[]{SpotsResource1, 324},
			new object[]{SpotsResource2, 272},
			new object[]{SpotsResource3, 405},
		};

		[Test, TestCaseSource("AddSpots_TestCaseSource")]
		public void AddSpots_should_add_spots(string input, int count)
		{
			//arrange
			//act
			ArcgisService.AddSpots(input);
			//assert
			SpotRepository.Received(count).AddSpot(Arg.Any<Spot>());
        }

        #region [CheckIfSpotBelongsToParcel]
        [Test]
        public void PointInPolygon_innerPoint_containedWithinPolygon()
        {
            Point[] vertices = new Point[4]  
                                {  
                                    new Point(){ Longitude = 1, Latitude = 3 },  
                                    new Point(){ Longitude = 1, Latitude = 1 },  
                                    new Point(){ Longitude = 4, Latitude = 1 },  
                                    new Point(){ Longitude = 4, Latitude = 3 }  
                                };

//            Point[] vertices = new Point[4]  
//                                {  
//                                    new Point(){ Longitude = (decimal)40.584791 , Latitude = (decimal)-111.593694 },  
//                                    new Point(){ Longitude = (decimal)40.584958 , Latitude = (decimal)-111.593408 },  
//                                    new Point(){ Longitude = (decimal)40.584740 , Latitude = (decimal)-111.593189 },  
//                                    new Point(){ Longitude = (decimal)40.584573 , Latitude = (decimal)-111.593475 }
//                                };

            var p = new Polygon(vertices);

           //Assert.AreEqual(true, p.PointInPolygon(new Point() { Longitude = (decimal)40.584691, Latitude = (decimal)-111.593394 }));
          Assert.AreEqual(true, p.PointInPolygon(new Point() { Longitude = 2, Latitude = 2 }));
        }

        [Test]
        public void PointInPolygon_outerPoint_notContainedWithinPolygon()
        {
            Point[] vertices = new Point[4]  
                                {  
                                    new Point() { Longitude = 1, Latitude = 3 },  
                                    new Point() { Longitude = 1, Latitude = 1 },  
                                    new Point() { Longitude = 4, Latitude = 1 },  
                                    new Point() { Longitude = 4, Latitude = 3 }
                                };

            Polygon p = new Polygon(vertices);

            Assert.AreEqual(false, p.PointInPolygon(new Point() { Longitude = 5, Latitude = 3 }));
        }

//        [Test]
//        public void PointInPolygon_DiagonalPointWithin()
//        {
//            var vertices = new PointF[3]  
//                                {  
//                                    new PointF(1, 3),  
//                                    new PointF(1, 1),  
//                                    new PointF(4, 1)  
//                                };
//
//            var p = new Polygon(vertices);
//
//            Assert.AreEqual(true, p.PointInPolygon(new PointF(2, 2)));
//        }
//
//        [Test]
//        public void PointInPolygon_DiagonalPointOut()
//        {
//            var vertices = new PointF[3]  
//                                {  
//                                    new PointF(1, 3),  
//                                    new PointF(1, 1),  
//                                    new PointF(4, 1)  
//                                };
//
//            var p = new Polygon(vertices);
//
//            Assert.AreEqual(false, p.PointInPolygon(new PointF(3, 3)));
//        }
//
//        [Test]
//        public void PointInPolygon_PerformanceTest()
//        {
//            var vertices = new PointF[4]  
//                                {  
//                                    new PointF(1, 3),  
//                                    new PointF(1, 1),  
//                                    new PointF(4, 1),  
//                                    new PointF(4, 3)  
//                                };
//
//            var p = new Polygon(vertices);
//
//            var sw = new Stopwatch();
//            sw.Start();
//
//            for (var i = 0; i < 200000; i++)
//                p.PointInPolygon(new PointF(2, 2));
//
//            sw.Stop();
//
//            Assert.IsTrue(sw.Elapsed.TotalSeconds < 1);
//        }  
        #endregion

    }
}
