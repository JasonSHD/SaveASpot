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

		private static string ParcelsResource1 { get { return ""; } }
		private static string ParcelsResource2 { get { return ""; } }
		private static string ParcelsResource3 { get { return ""; } }

		public static object[] AddParcelsForParcels_TestCaseSource = new object[]
		{
			new object[]{ParcelsResource1, 10},
			new object[]{ParcelsResource2, 2},
			new object[]{ParcelsResource3, 12},
		};

		[Test, TestCaseSource("AddParcelsForParcels_TestCaseSource")]
		public void AddParcels_should_add_parcels(string input, int count)
		{
			//arrange
			//act
			ArcgisService.AddParcels(input);
			//assert
			ParcelRepository.Received().AddParcel(Arg.Any<Parcel>());
		}

		public static object[] AddParcelsForPhases_TestCaseSource = new object[]
		{
			new object[]{ParcelsResource1, 3},
			new object[]{ParcelsResource2, 4},
			new object[]{ParcelsResource3, 5},
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

		private static string ParcelsResource4 { get { return ""; } }
		private static string ParcelsResource5 { get { return ""; } }
		private static string ParcelsResource6 { get { return ""; } }

		public static object[] AddSpots_TestCaseSource = new object[]
		{
			new object[]{ParcelsResource4, 3},
			new object[]{ParcelsResource5, 4},
			new object[]{ParcelsResource6, 3},
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
