using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using SaveASpot.Data.Models;
using ClipperLib;

namespace SaveASpot.Areas.API.Controllers
{
    public class SpotController : SaveASpot.Controllers.ApplicationController
    {
        public JsonResult CountByPhase(ObjectId id)
        {
            int count = 0;
            bool success = true;

            try
            {
                var result = Context.Spots.GetSpotsInfoByPhase(id);
                count = result.Total;
            }
            catch { success = false; }

            return Json(new { success = success, count = count });
        }

        public JsonResult GetByPhase(ObjectId id, int index, int take, double neLat, double neLng, double swLat, double swLng)
        {
            bool success = true;
            List<Spot> results = new List<Spot>();

            try
            {
                // now grab the range of spots
                results = Context.Spots.GetSpotsByPhase(id, take, index,
                    new Coordinate { Latitude = neLat, Longitude = neLng },
                    new Coordinate { Latitude = swLat, Longitude = swLng });
            }
            catch { success = false; }

            return Json(new { success = success, results = results });
        }

        public JsonResult GetByPhaseAndBounds(ObjectId id, double neLat, double neLng, double swLat, double swLng)
        {
            bool success = true;
            int count = 0;
            List<Spot> results = new List<Spot>();

            try
            {
                // now grab the range of spots
                count = Context.Spots.GetSpotCountByRegion(id,
                    new Coordinate { Latitude = neLat, Longitude = neLng },
                    new Coordinate { Latitude = swLat, Longitude = swLng });
            }
            catch { success = false; }

            return Json(new { success = success, results = results, count = count });
        }

        public JsonResult GetBySponsorOld(ObjectId id)
        {
            bool success = true;
            List<Spot> spots = new List<Spot>();
            try
            {
                spots = Context.Spots.GetSpotsBySponsor(id);
            }
            catch { success = false; }

            return Json(new { success = success, results = spots });
        }

        public JsonResult GetBySponsor(ObjectId id)
        {
            var spots = Context.SponsorSpots.GetSpotsBySponsors(id);
            var sponsor = Context.Sponsors.GetSponsor(id);

            Clipper clipper = new Clipper();
            var polygons = new List<List<IntPoint>>();
            var scale = 100000000.0;

            foreach (var spot in spots)
            {
                var polygon = new List<IntPoint>();

                foreach (var coord in spot.SpotShape)
                {
                    polygon.Add(new IntPoint(coord.Longitude * scale, coord.Latitude * scale));
                }
                polygons.Add(polygon);
            }

            var solution = new List<List<IntPoint>>();

            clipper.AddPaths(polygons, PolyType.ptSubject, true);
            clipper.Execute(ClipType.ctUnion, solution,
                PolyFillType.pftNonZero, PolyFillType.pftNonZero);

            var results = new List<Spot>();

            foreach (var shape in solution)
            {
                var resultShape = new Spot();
                foreach (var item in shape)
                {
                    resultShape.SpotShape.Add(new Coordinate { Latitude = item.Y / scale, Longitude = item.X / scale });
                }
                results.Add(resultShape);
            }

            /*var coordinateList = new List<Coordinate>();
            //var swList = new List<Coordinate>();

            foreach (var r in spots)
            {
                Coordinate northEast = new Coordinate()
                {
                    Latitude = r.SpotShape.Max(s => s.Latitude),
                    Longitude = r.SpotShape.Max(s => s.Longitude)
                };
                coordinateList.Add(northEast);
                Coordinate southWest = new Coordinate()
                {
                    Latitude = r.SpotShape.Min(s => s.Latitude),
                    Longitude = r.SpotShape.Min(s => s.Longitude)
                };
                coordinateList.Add(southWest);
            }

            var latMax = coordinateList.Max(s => s.Latitude);
            var latMin = coordinateList.Min(s => s.Latitude);
            var lngMax = coordinateList.Max(s => s.Longitude);
            var lngMin = coordinateList.Min(s => s.Longitude);

            sponsor.Center.Latitude = (latMax+ latMin)/2;
            sponsor.Center.Longitude = (lngMax + lngMin) / 2;

            Context.Sponsors.Update(sponsor);*/

            return Json(new { success = true, results = results }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Phases()
        {
            var phases = Context.Phases.GetAllPhases();
            bool success = true;

            foreach (var phase in phases)
            {
                var parcels = Context.Parcels.GetParcelsByPhaseID(phase.PhaseID);

                if (parcels != null)
                {
                    foreach (var parcel in parcels)
                    {
                        success = Context.Spots.UpdateSpotsWithPhaseID(phase.PhaseID, parcel.ParcelID);
                        if (!success) { break; };
                    }
                }
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }
    }
}
