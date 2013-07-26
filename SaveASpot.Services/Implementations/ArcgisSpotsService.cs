using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.CSharp.RuntimeBinder;
using MongoDB.Bson;
using Newtonsoft.Json;
using SaveASpot.Core;
using SaveASpot.Core.Logging;
using SaveASpot.Repositories.Interfaces.PhasesAndParcels;
using SaveASpot.Repositories.Models;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Services.Implementations
{
	public sealed class ArcgisSpotsService : ISpotsService
	{
		private readonly IParcelQueryable _parcelQueryable;
		private readonly ILogger _logger;
		private readonly ISpotRepository _spotRepository;

		public ArcgisSpotsService(IParcelQueryable parcelQueryable, ILogger logger, ISpotRepository spotRepository)
		{
			_parcelQueryable = parcelQueryable;
			_logger = logger;
			_spotRepository = spotRepository;
		}

		public IMethodResult<MessageResult> AddSpots(StreamReader input)
		{
			var jsonSpots = input.ReadToEnd();
			dynamic array;
			try
			{
				array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonSpots));
			}
			catch (JsonReaderException)
			{
				return new MessageMethodResult(false, "NotRecognizeJsonForSpot");
			}

			try
			{
				IEnumerable<Parcel> parcels = _parcelQueryable.Find(_parcelQueryable.All()).ToList();
				ObjectId parcelId = default(ObjectId);

				if (!parcels.Any())
				{
					_logger.Info(string.Format("{0}", "Can not add Spot because there are no one Parcels to association with"));
					throw new RowNotInTableException("Can not add Spot because there are no one Parcels to association with");
				}

				foreach (var col in array)
				{
					foreach (var featuresCol in col.features)
					{
						var spotPoints = new List<Point>();

						foreach (var el in featuresCol.geometry.coordinates[0])
						{
							spotPoints.Add(new Point { Latitude = el[0], Longitude = el[1] });
						}

						foreach (var parcel in parcels)
						{
							bool spotIsInside = IfSpotIsInsideParcel(parcel.ParcelShape, spotPoints);
							if (spotIsInside)
							{
								parcelId = parcel.Id;
							}
						}

						_spotRepository.AddSpot(new Spot
																			{
																				SpotLength = featuresCol.properties.Shape_Leng,
																				SpotArea = featuresCol.properties.Shape_Area,
																				ParcelId = parcelId,
																				SpotShape = spotPoints
																			});
					}

				}
			}
			catch (RuntimeBinderException)
			{
				return new MessageMethodResult(false, "NotRecognizeObjectForSpot");
			}

			return new MessageMethodResult(true, string.Empty);
		}

		private bool IfSpotIsInsideParcel(IEnumerable<Point> parcelVerticesArg, IEnumerable<Point> spotVertices)
		{
			bool wholeSpotIsInside = true;

			var parcelVertices = parcelVerticesArg.ToList();

			List<Point> parcelVerticesWithoutLast = parcelVertices.ToList();
			parcelVerticesWithoutLast.RemoveAt(parcelVertices.Count() - 1);

			List<Point> spotVerticesWithoutLast = spotVertices.ToList();
			spotVerticesWithoutLast.RemoveAt(spotVertices.Count() - 1);

			Polygon parcelPolygon = new Polygon(parcelVerticesWithoutLast.ToArray());

			foreach (var spotPoint in spotVerticesWithoutLast)
			{
				bool currentSpotVerticeInside = parcelPolygon.PointInPolygon(spotPoint);
				if (!currentSpotVerticeInside)
				{
					wholeSpotIsInside = false;
				}
				if (!wholeSpotIsInside)
				{
					break;
				}
			}

			return wholeSpotIsInside;
		}
	}
}