using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Areas.Settings.Models;
using MongoDB.Bson;
using SaveASpot.Data.Models;


namespace SaveASpot.Areas.Settings.Controllers
{
    public class SponsorController : SaveASpot.Controllers.ApplicationController
    {
        public ActionResult Index()
        {
            var model = new SponsorList();
            model.Sponsors = Context.Sponsors.GetAllSponsors();
            return View(model);
        }

        public ActionResult New()
        {
            var model = new Sponsor();

            ViewBag.Error = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult New(Sponsor model)
        {
            ViewBag.Error = true;

            if (ModelState.IsValid)
            {
                Sponsor result = Context.Sponsors.Insert(model);

                if (result != null)
                {
                    return RedirectToAction("Edit", new { id = result.SponsorID });
                }
            }

            return View(model);
        }
    }
}
