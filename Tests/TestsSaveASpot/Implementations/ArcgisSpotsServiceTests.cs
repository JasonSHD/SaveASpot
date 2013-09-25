using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using NSubstitute;
using NUnit.Framework;
using SaveASpot.Core.Geocoding;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations.PhasesAndParcels;
using TestsSaveASpot.Core;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class ArcgisSpotsServiceTests
	{
		private ISpotRepository SpotRepository { get; set; }
		private IParcelQueryable ParcelQueryable { get; set; }
		private ArcgisSpotsParceService Target { get; set; }

		[SetUp]
		public void SetUp()
		{
			SpotRepository = Substitute.For<ISpotRepository>();
			ParcelQueryable = Substitute.For<IParcelQueryable>();

			const int maxLongitude = 1000,
				maxLatitude = 1000;

			ParcelQueryable.Find(Arg.Any<IParcelFilter>()).Returns(new[] { new Parcel
			{
				Id = ObjectId.GenerateNewId(), 
				ParcelShape = new List<Point>
					              {
						              new Point{Longitude = maxLongitude *-1, Latitude = maxLatitude *-1},
													new Point{Longitude = maxLongitude *-1, Latitude = maxLatitude},
													new Point{Longitude = maxLongitude, Latitude = maxLatitude},
													new Point{Longitude = maxLongitude, Latitude = maxLatitude *-1}
					              }
			} });
			Target = new ArcgisSpotsParceService(ParcelQueryable, new LoggerStub(), SpotRepository);
		}

		private static StreamReader SpotsResource1
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase1_Grid.json"); }
		}
		private static StreamReader SpotsResource2
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase3_Grid.json"); }
		}
		private static StreamReader SpotsResource3
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase4_Grid.json"); }
		}

		public static object[] AddSpots_TestCaseSource = new object[]
			                                                 {
				                                                 new object[]{SpotsResource1, 324},
				                                                 new object[]{SpotsResource2, 272},
				                                                 new object[]{SpotsResource3, 405},
			                                                 };

		[Test, TestCaseSource("AddSpots_TestCaseSource")]
		public void AddSpots_should_add_spots(StreamReader input, int count)
		{
			//arrange
			//act
			Target.AddSpots(input);
			//assert
			SpotRepository.Received(count).AddSpot(Arg.Any<Spot>());
		}
	}
}