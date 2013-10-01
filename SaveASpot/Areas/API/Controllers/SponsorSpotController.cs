using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using SaveASpot.Data.Models;

namespace SaveASpot.Areas.API.Controllers
{
    public class SponsorSpotController : SaveASpot.Controllers.ApplicationController
    {
        public JsonResult CountByPhase(ObjectId id)
        {
            int count = 0;
            bool success = true;

            try
            {
                var result = Context.SponsorSpots.GetSpotsInfoByPhase(id);
                count = result.Total;
            }
            catch { success = false; }

            return Json(new { success = success, count = count });
        }

        public JsonResult GetByPhase(ObjectId id, int index, int take)
        {
            bool success = true;
            List<SponsorSpot> results = new List<SponsorSpot>();

            try
            {
                // now grab the range of spots
                results = Context.SponsorSpots.GetSpotsByPhase(id, take, index);
            }
            catch { success = false; }

            return Json(new { success = success, results = results });
        }
    }
}
