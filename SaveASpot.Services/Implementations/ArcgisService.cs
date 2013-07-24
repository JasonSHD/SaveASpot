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
        #region [Fields]
        private readonly IPhaseRepository _phaseRepository;
        private readonly IParcelRepository _parcelRepository;
        private readonly ISpotRepository _spotRepository;
        #endregion

        #region [Constructors]
        public ArcgisService(IParcelRepository parcelRepository, IPhaseRepository phaseRepository,ISpotRepository spotRepository)
        {
            _parcelRepository = parcelRepository;
            _phaseRepository = phaseRepository;
            _spotRepository = spotRepository;
        }
        #endregion

        #region [IArcgisService implementation]
        public IMethodResult<MessageResult> AddParcels(string jsonParcels)
        {
            dynamic array = default(dynamic);
            try
            {
                array = JsonConvert.DeserializeObject(string.Format("[{0}]", jsonParcels));
            }
            catch (JsonReaderException ex)
            {
                return new MessageMethodResult(false,
                                               "Can not recognize json format when reading loaded Parcel document");
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
        #endregion

        #region [Methods]
        public bool IfSpotIsInsideParcel(IEnumerable<Point> spotVertices, IEnumerable<Point> parcelVertices)
        {
            return true;
        }
        #endregion
    }


    public sealed class Polygon
    {
        private readonly Point[] _vertices;
        public Polygon(Point[] vertices)
        {
            _vertices = vertices;
        }

        /// <summary>  
        ///     Determines if the specified <see cref="Point"/> if within this polygon.  
        /// </summary>  
        /// <param name="point">  
        ///     The point containing the x(Longitude), y(Latitude) coordinates to check.  
        /// </param>  
        public bool PointInPolygon(Point point)
        {
            bool isInside = false;
            for (int i = 0, j = _vertices.Length - 1; i < _vertices.Length; j = i++)
            {
                //check if this point is between Y coordinates of polygon
                if (_vertices[i].Latitude < point.Latitude && _vertices[j].Latitude >= point.Latitude ||
                    _vertices[j].Latitude < point.Latitude && _vertices[i].Latitude >= point.Latitude)
                {
                    //if it is between Y coordinates, check if point is beetween X coordinates of this polygon
                    if (_vertices[i].Longitude + (point.Latitude - _vertices[i].Latitude) / (_vertices[j].Latitude - _vertices[i].Latitude) * (_vertices[j].Longitude - _vertices[i].Longitude) < point.Longitude)
                    {
                        isInside = !isInside;
                    }
                }
            }
            return isInside;
        }
    }
}


