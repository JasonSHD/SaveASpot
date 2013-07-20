using System.Collections.Generic;
using Newtonsoft.Json;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Services.Interfaces;
using SaveASpot.Repositories.Models;

namespace SaveASpot.Services.Implementations
{
	public sealed class ArcgisService : IArcgisService
	{
		private readonly IPhaseRepository _phaseRepository;
		private readonly IParcelRepository _parcelRepository;
		private readonly ISpotRepository _spotRepository;

		public ArcgisService (IParcelRepository parcelRepository, IPhaseRepository phaseRepository, ISpotRepository spotRepository)
		{
			_parcelRepository = parcelRepository;
			_phaseRepository = phaseRepository;
			_spotRepository = spotRepository;
		}

		public void AddParcels(string jsonParcels)
		{
			dynamic array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonParcels));
			foreach (var col in array)
			{
				foreach (var featuresCol in col.features)
				{
					var phase = new Phase
					{
						PhaseName = featuresCol.properties.Phase
					};

					if (!_phaseRepository.PhaseExists(phase))
						_phaseRepository.AddPhase(phase);

					var points = new List<Point>();

					foreach (var el in featuresCol.geometry.coordinates[0])
					{
						points.Add(new Point { Latitude = el[0], Longitude = el[1] });
					}

					_parcelRepository.AddParcel(new Parcel
						                            {
							                            ParcelName = featuresCol.properties.Name,
							                            ParcelLength = featuresCol.properties.Shape_Leng,
							                            ParcelArea = featuresCol.properties.Shape_Area,
							                            ParcelAcres = featuresCol.properties.Acres,
							                            ParcelShape = points
						                            });
				}

			}
		}

		public void AddSpots(string jsonSpots)
		{
			dynamic array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonSpots));
			foreach (var col in array)
			{
				foreach (var featuresCol in col.features)
				{
					var points = new List<Point>();

					foreach (var el in featuresCol.geometry.coordinates[0])
					{
						points.Add(new Point { Latitude = el[0], Longitude = el[1] });
					}

					_spotRepository.AddSpot(new Spot
					{
						SpotLength = featuresCol.properties.Shape_Leng,
						SpotArea = featuresCol.properties.Shape_Area,
						SpotShape = points
					});
				}

			}
		}
	}
}
