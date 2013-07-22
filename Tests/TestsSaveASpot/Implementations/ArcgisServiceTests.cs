using System.IO;
using NSubstitute;
using NUnit.Framework;
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

		[SetUp]
		public void SetUp()
		{
			ParcelRepository = Substitute.For<IParcelRepository>();
			PhaseRepository = Substitute.For<IPhaseRepository>();
			SpotRepository = Substitute.For<ISpotRepository>();

			ArcgisService = new ArcgisService(ParcelRepository, PhaseRepository, SpotRepository);
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
	}
}
