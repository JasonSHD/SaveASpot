using System.IO;
using NSubstitute;
using NUnit.Framework;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class ArcgisParcelsServiceTests
	{
		private ArcgisParcelsService Target { get; set; }
		private IParcelRepository ParcelRepository { get; set; }
		private IPhaseRepository PhaseRepository { get; set; }
		private IPhaseQueryable PhaseQueryable { get; set; }

		[SetUp]
		public void SetUp()
		{
			ParcelRepository = Substitute.For<IParcelRepository>();
			PhaseRepository = Substitute.For<IPhaseRepository>();
			PhaseQueryable = Substitute.For<IPhaseQueryable>();
			Target = new ArcgisParcelsService(PhaseRepository, PhaseQueryable, ParcelRepository);
		}

		private static StreamReader ParcelsResource1
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase1.json"); }
		}
		private static StreamReader ParcelsResource2
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase3.json"); }
		}
		private static StreamReader ParcelsResource3
		{
			get { return new StreamReader(@"..\..\..\..\SaveASpot\Content\data\Phase4.json"); }
		}

		public static object[] AddParcelsForParcels_TestCaseSource = new object[]
			                                                             {
				                                                             new object[]{ParcelsResource1, 4},
				                                                             new object[]{ParcelsResource2, 6},
				                                                             new object[]{ParcelsResource3, 8},
			                                                             };

		[Test, TestCaseSource("AddParcelsForParcels_TestCaseSource")]
		public void AddParcels_should_add_parcels(StreamReader input, int count)
		{
			//arrange
			//act
			Target.AddParcels(input);
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
		public void AddParcels_should_add_phases(StreamReader input, int count)
		{
			//arrange
			//act
			Target.AddParcels(input);
			//assert
			PhaseRepository.Received(count).AddPhase(Arg.Any<Phase>());
		}
	}
}