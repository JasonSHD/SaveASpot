using System.Collections.Generic;
using System.IO;
using System.Linq;
using MongoDB.Bson;
using NSubstitute;
using NUnit.Framework;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Implementations.PhasesAndParcels;

namespace TestsSaveASpot.Implementations
{
	[TestFixture]
	public sealed class ArcgisParcelsServiceTests
	{
		private ArcgisParcelsParceService Target { get; set; }
		private IParcelRepository ParcelRepository { get; set; }
		private IPhaseRepository PhaseRepository { get; set; }
		private IPhaseQueryable PhaseQueryable { get; set; }

		[SetUp]
		public void SetUp()
		{
			ParcelRepository = Substitute.For<IParcelRepository>();
			PhaseRepository = Substitute.For<IPhaseRepository>();
			PhaseQueryable = Substitute.For<IPhaseQueryable>();
			Target = new ArcgisParcelsParceService(PhaseRepository, PhaseQueryable, ParcelRepository);
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
			Target.AddParcels(input, default(decimal));
			//assert
			ParcelRepository.Received(count).AddParcel(Arg.Any<Parcel>());
		}

		public static object[] AddParcelsForPhases_TestCaseSource = new object[]
			                                                            {
				                                                            new object[]{ParcelsResource1, 1},
				                                                            new object[]{ParcelsResource2, 1},
				                                                            new object[]{ParcelsResource3, 1},
			                                                            };

		[Test, TestCaseSource("AddParcelsForPhases_TestCaseSource")]
		public void AddParcels_should_add_phases(StreamReader input, int count)
		{
			//arrange
			var phases = new List<string>();
			PhaseQueryable.Find(Arg.Is<PhaseFilter>(f => phases.Any(e => e == f.Name))).Returns(new[] { new Phase { Id = ObjectId.GenerateNewId() } });
			PhaseRepository.AddPhase(Arg.Do<Phase>(p => phases.Add(p.PhaseName)));
			PhaseQueryable.ByName(Arg.Any<string>()).Returns(c => new PhaseFilter { Name = c.Arg<string>() });
			//act
			Target.AddParcels(input, default(decimal));
			//assert
			PhaseRepository.Received(count).AddPhase(Arg.Any<Phase>());
		}

		private sealed class PhaseFilter : IPhaseFilter
		{
			public string Name { get; set; }
		}
	}
}