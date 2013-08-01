﻿using System.IO;
using MongoDB.Bson;
using NSubstitute;
using NUnit.Framework;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations;
using TestsSaveASpot.Core;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class ArcgisSpotsServiceTests
	{
		private ISpotRepository SpotRepository { get; set; }
		private IParcelQueryable ParcelQueryable { get; set; }
		private ArcgisSpotsService Target { get; set; }

		[SetUp]
		public void SetUp()
		{
			SpotRepository = Substitute.For<ISpotRepository>();
			ParcelQueryable = Substitute.For<IParcelQueryable>();
			ParcelQueryable.Find(Arg.Any<IParcelFilter>()).Returns(new[] { new Parcel { Id = ObjectId.GenerateNewId() } });
			Target = new ArcgisSpotsService(ParcelQueryable, new LoggerStub(), SpotRepository);
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