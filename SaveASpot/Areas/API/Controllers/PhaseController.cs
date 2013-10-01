using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Data.Models;

namespace SaveASpot.Areas.API.Controllers
{
    public class PhaseController : SaveASpot.Controllers.ApplicationController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult All()
        {
            var phases = new List<Phase>();
            bool success = true;
            try
            {
                phases = Context.Phases.GetAllPhases();

                foreach (var phase in phases)
                {
                    phase.Parcels = Context.Parcels.GetParcelsByPhaseID(phase.PhaseID);
                }
            }
            catch (Exception exp)
            {
                success = false;
            }

            return Json(new { success = success, results = phases });
        }
    }
}
