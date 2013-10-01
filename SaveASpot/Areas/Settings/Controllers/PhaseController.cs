using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using SaveASpot.Areas.Settings.Models;
using MongoDB.Bson;
using SaveASpot.Data.Models;

namespace SaveASpot.Areas.Settings.Controllers
{
    public class PhaseController : SaveASpot.Controllers.ApplicationController
    {
        public ActionResult Index()
        {
            var model = new PhaseList();
            model.Phases = Context.Phases.GetAllPhases();
            return View(model);
        }

        public ActionResult Edit(ObjectId id)
        {
            var model = new PhaseDetail();
            model.Phase = Context.Phases.GetPhase(id);

            // get parcel information
            model.Phase.Parcels = Context.Parcels.GetParcelsByPhaseID(id) ?? new List<Parcel>();

            // get spot information
            model.SpotInfo = Context.Spots.GetSpotsInfoByPhase(id);

            // get sponsor information
            model.SponsorInfo = Context.SponsorSpots.GetSpotsInfoByPhase(id);

            ViewBag.Success = false;
            ViewBag.Error = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PhaseDetail model)
        {
            ViewBag.Success = false;
            ViewBag.Error = true;

            if (ModelState.IsValid)
            {
                bool success = Context.Phases.Update(model.Phase);

                if (success)
                {
                    ViewBag.Success = true;
                    ViewBag.Error = false;
                }
            }

            // get parcel information
            model.Phase.Parcels = Context.Parcels.GetParcelsByPhaseID(model.Phase.PhaseID) ?? new List<Parcel>();

            // get spot information
            model.SpotInfo = Context.Spots.GetSpotsInfoByPhase(model.Phase.PhaseID);

            // get sponsor information
            model.SponsorInfo = Context.SponsorSpots.GetSpotsInfoByPhase(model.Phase.PhaseID);

            return View(model);
        }

        public ActionResult New()
        {
            var model = new Phase();

            ViewBag.Error = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Phase model)
        {
            ViewBag.Error = true;

            if (ModelState.IsValid)
            {
                Phase result = Context.Phases.Insert(model);

                if (result != null)
                {
                    return RedirectToAction("Edit", new { id = result.PhaseID });
                }
            }

            return View(model);
        }

        public JsonResult Delete(ObjectId id)
        {
            bool success = true;
            try
            {
                List<Parcel> parcels = Context.Parcels.GetParcelsByPhaseID(id);

                success = Context.Phases.Delete(id) && Context.Parcels.DeleteByPhaseID(id);

                if (parcels != null)
                {
                    foreach (var parcel in parcels)
                    {
                        success = success && Context.Spots.DeleteByParcelId(parcel.ParcelID);
                    }
                }
            }
            catch
            {
                success = false;
            }

            return Json(new { success = success });
        }

        public JsonResult UploadPhase(ObjectId id, HttpPostedFileBase shapeFile)
        {
            bool success = true;

            try
            {
                dynamic file = LoadFile(shapeFile);

                if (file != null)
                {
                    // remove old parcels
                    Context.Parcels.DeleteByPhaseID(id);

                    // add the new parcels
                    List<Parcel> parcels = new List<Parcel>();

                    foreach (var feature in file.features)
                    {
                        Parcel parcel = new Parcel()
                        {
                            ParcelName = feature.properties.Name,
                            ParcelLength = feature.properties.Shape_Leng,
                            ParcelAcres = feature.properties.Acres,
                            ParcelArea = feature.properties.Shape_Area,
                            PhaseId = id
                        };

                        foreach (var coord in feature.geometry.coordinates[0])
                        {
                            parcel.ParcelShape.Add(new Coordinate { Latitude = coord[0], Longitude = coord[1] });
                        }

                        parcels.Add(parcel);
                    }


                    success = Context.Parcels.InsertBatch(parcels);
                }
            }
            catch { success = false; }

            return Json(new { success = success });
        }

        public JsonResult UploadSpots(ObjectId id, HttpPostedFileBase shapeFile)
        {
            bool success = true;

            try
            {
                dynamic file = LoadFile(shapeFile);

                if (file != null)
                {
                    // remove old parcels
                    Context.Spots.DeleteByPhaseID(id);

                    // add the new parcels
                    List<Spot> spots = new List<Spot>();

                    foreach (var feature in file.features)
                    {
                        Spot spot = new Spot()
                        {
                            PhaseID = id
                        };

                        foreach (var coord in feature.geometry.coordinates[0])
                        {
                            spot.SpotShape.Add(new Coordinate { Latitude = coord[0], Longitude = coord[1] });
                        }

                        spots.Add(spot);
                    }

                    success = Context.Spots.InsertBatch(spots);
                }
            }
            catch { success = false; }

            return Json(new { success = success });
        }

        public JsonResult UploadSponsors(ObjectId id, HttpPostedFileBase shapeFile)
        {
            bool success = true;

            try
            {
                dynamic file = LoadFile(shapeFile);

                if (file != null)
                {
                    // remove old parcels
                    Context.SponsorSpots.DeleteByPhaseID(id);

                    // add the new parcels
                    List<SponsorSpot> spots = new List<SponsorSpot>();

                    foreach (var feature in file.features)
                    {
                        SponsorSpot spot = new SponsorSpot()
                        {
                            PhaseID = id
                        };

                        foreach (var coord in feature.geometry.coordinates[0])
                        {
                            spot.SpotShape.Add(new Coordinate { Latitude = coord[0], Longitude = coord[1] });
                        }

                        spots.Add(spot);
                    }

                    success = Context.SponsorSpots.InsertBatch(spots);
                }
            }
            catch { success = false; }

            return Json(new { success = success });
        }

        #region helper methods

        private dynamic LoadFile(HttpPostedFileBase shapeFile)
        {
            try
            {
                if (shapeFile != null && shapeFile.ContentLength > 0 && shapeFile.ContentType == "application/json")
                {
                    using (StreamReader stream = new StreamReader(shapeFile.InputStream))
                    {
                        var output = stream.ReadToEnd();
                        return JsonConvert.DeserializeObject(output);
                    }
                }
            }
            catch { }

            return null;
        }

        #endregion
    }
}
