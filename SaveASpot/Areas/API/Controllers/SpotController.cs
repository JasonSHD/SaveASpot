using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using SaveASpot.Data.Models;

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

        public JsonResult GetByPhase(ObjectId id, int index, int take)
        {
            bool success = true;
            List<Spot> results = new List<Spot>();

            try
            {
                // now grab the range of spots
                results = Context.Spots.GetSpotsByPhase(id, take, index);
            }
            catch { success = false; }

            return Json(new { success = success, results = results });
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
