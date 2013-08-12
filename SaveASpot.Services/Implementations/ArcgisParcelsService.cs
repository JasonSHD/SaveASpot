using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class ArcgisParcelsService : IParcelsService
	{
		private readonly IPhaseRepository _phaseRepository;
		private readonly IPhaseQueryable _phaseQueryable;
		private readonly IParcelRepository _parcelRepository;

		public ArcgisParcelsService(IPhaseRepository phaseRepository, IPhaseQueryable phaseQueryable, IParcelRepository parcelRepository)
		{
			_phaseRepository = phaseRepository;
			_phaseQueryable = phaseQueryable;
			_parcelRepository = parcelRepository;
		}

		public IMethodResult<MessageResult> AddParcels(StreamReader input)
		{
			var jsonParcels = input.ReadToEnd();
			dynamic array;
			try
			{
				array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonParcels));
			}
			catch (JsonReaderException)
			{
				return new MessageMethodResult(false,
																			 "Can not recognize json format when reading loaded Parcel document");
			}

			try
			{
				foreach (var col in array)
				{
					foreach (var featuresCol in col["features"])
					{
						var phase = new Phase
													{
														PhaseName = (string)featuresCol["properties"]["Phase"]
													};

						if (!_phaseQueryable.Find(_phaseQueryable.ByName(phase.PhaseName)).Any())
							_phaseRepository.AddPhase(phase);

						phase = _phaseQueryable.Find(_phaseQueryable.ByName(phase.PhaseName)).First();

						var points = new List<Point>();

						foreach (var el in featuresCol["geometry"]["coordinates"][0])
						{
							points.Add(new Point { Latitude = (decimal)el[0], Longitude = (decimal)el[1] });
						}

						_parcelRepository.AddParcel(new Parcel
																					{
																						ParcelName = (string)featuresCol["properties"]["Name"],
																						ParcelLength = (decimal)featuresCol["properties"]["Shape_Leng"],
																						ParcelArea = (decimal)featuresCol["properties"]["Shape_Area"],
																						ParcelAcres = (decimal)featuresCol["properties"]["Acres"],
																						ParcelShape = points,
																						PhaseId = phase.Id
																					});
					}

				}
			}
			catch (RuntimeBinderException)
			{
				return new MessageMethodResult(false, "Can not create object from loaded Parcel document");
			}

			return new MessageMethodResult(true, string.Empty);
		}
	}
}