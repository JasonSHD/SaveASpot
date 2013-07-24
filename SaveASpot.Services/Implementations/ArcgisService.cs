using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SaveASpot.Core;
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

		public ArcgisService(IParcelRepository parcelRepository, IPhaseRepository phaseRepository, ISpotRepository spotRepository)
		{
			_parcelRepository = parcelRepository;
			_phaseRepository = phaseRepository;
			_spotRepository = spotRepository;
		}

		public IMethodResult<MessageResult> AddParcels(string jsonParcels)
		{
			dynamic array = default(dynamic);
			try
			{
				array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonParcels));
			}
			catch (JsonReaderException ex)
			{
				return new MessageMethodResult(false, "Can not recognize json format when reading loaded Parcel document");
			}

			try
			{
				foreach (var col in array)
				{
					foreach (var featuresCol in col.features)
					{
						var phase = new Phase
						{
							PhaseName = featuresCol.properties.Phase
						};

						if (_phaseRepository.GetPhaseByName(phase.PhaseName) == null)
							_phaseRepository.AddPhase(phase);

						var phaseResult = _phaseRepository.GetPhaseByName(phase.PhaseName);

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
							ParcelShape = points,
							PhaseId = phaseResult.Identity
						});
					}

				}
			}
			catch (RuntimeBinderException ex)
			{
				return new MessageMethodResult(false, "Can not create object from loaded Parcel document");
			}


			return new MessageMethodResult(true, string.Empty);
		}

		public IMethodResult<MessageResult> AddSpots(string jsonSpots)
		{
			dynamic array = default(dynamic);
			try
			{
				array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonSpots));
			}
			catch (JsonReaderException ex)
			{
				return new MessageMethodResult(false, "Can not recognize json format when loading Spot document");
			}

			try
			{
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
			catch (RuntimeBinderException ex)
			{
				return new MessageMethodResult(false, "Can not create object from loaded Spot document");
			}

			return new MessageMethodResult(true, string.Empty);
		}
	}
}
