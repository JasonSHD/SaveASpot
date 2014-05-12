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
    [Authorize(Roles = "Admin")]
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

        public ActionResult Edit(ObjectId id)
        {
            var model = new SponsorDetail();
            model.Sponsor = Context.Sponsors.GetSponsor(id);

            // first get an active phase
            var phase = Context.Phases.GetActivePhase();

            if (phase != null && phase.PhaseID != ObjectId.Empty)
            {
                // get sponsor information
                model.SponsorInfo = new Count();

                // get all sponsor spots
                model.SponsorInfo.Total = Context.SponsorSpots.GetCountByPhase(phase.PhaseID);

                // get sponsors spots
                model.SponsorInfo.Paid = Context.SponsorSpots.GetSpotsBySponsors(id).Count;

                // get spot information
                model.SpotInfo = new Count();// Context.Spots.GetSpotsInfoByPhase(id);

                var spots = Context.Spots.GetSpotsBySponsor(id);

                model.SpotInfo.Total = spots.Count;
                model.SpotInfo.Paid = spots.Where(s => s.CustomerId != null && s.CustomerId != ObjectId.Empty).Count();

            }

            ViewBag.Success = false;
            ViewBag.Error = false;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(SponsorDetail model)
        {
            ViewBag.Success = false;
            ViewBag.Error = true;

            if (ModelState.IsValid)
            {
                bool success = Context.Sponsors.Update(model.Sponsor);

                if (success)
                {
                    ViewBag.Success = true;
                    ViewBag.Error = false;
                }
            }

            // get spot information
            model.SpotInfo = new Count();// Context.Spots.GetSpotsInfoByPhase(id);

            // get sponsor information
            model.SponsorInfo = new Count();// Context.SponsorSpots.GetSpotsInfoByPhase(id);

            return View(model);
        }

        public JsonResult Delete(ObjectId id)
        {
            bool success = true;
            try
            {
                success = Context.Sponsors.Delete(id);
            }
            catch
            {
                success = false;
            }

            return Json(new { success = success });
        }

        public ActionResult Checkout(ObjectId id)
        {
            var cart = CurrentCart;
            cart.SponsorID = id;

            // get the phase info
            var phases = Context.Phases.GetAllPhases();

            foreach (var item in cart.Items)
            {
                var phase = (from p in phases where p.PhaseID == item.PhaseID select p);
                if (phase.Count() > 0)
                {
                    item.SponsorPrice = phase.First().SponsorPrice;
                }
            }

            return View(cart);
        }

        [HttpPost]
        public ActionResult Checkout(Cart model)
        {
            var cart = CurrentCart;

            foreach (var item in cart.Items)
            {
                var sponsorSpot = Context.SponsorSpots.GetSpot(item.SpotID);
                sponsorSpot.Taken = true;
                sponsorSpot.SponsorID = model.SponsorID;

                Context.SponsorSpots.SaveSpot(sponsorSpot);

                // process the sub spots below
                Coordinate northEast = new Coordinate()
                { 
                    Latitude = sponsorSpot.SpotShape.Max(s => s.Latitude), 
                    Longitude = sponsorSpot.SpotShape.Max(s => s.Longitude)
                };
                Coordinate southWest = new Coordinate()
                {
                    Latitude = sponsorSpot.SpotShape.Min(s => s.Latitude),
                    Longitude = sponsorSpot.SpotShape.Min(s => s.Longitude)
                };

                // get the spots and update them
                var spots = Context.Spots.GetSpotsByRegion(northEast, southWest);
                bool success = Context.Spots.UpdateSponsor(spots, sponsorSpot.SponsorID);
            }

            ClearCart();

            return RedirectToAction("Edit", new { id = model.SponsorID });
        }

        public JsonResult ClearSpots(ObjectId id)
        {
            bool success = true;
            try
            {
                var sponsorSpots = Context.SponsorSpots.GetSpotsBySponsors(id);
                foreach (var sponsor in sponsorSpots)
                {
                    sponsor.SponsorID = ObjectId.Empty;
                    sponsor.Taken = false;
                    success = success && Context.SponsorSpots.SaveSpot(sponsor);
                }

                var spots = Context.Spots.GetSpotsBySponsor(id);
                success = success && Context.Spots.UpdateSponsor(spots, ObjectId.Empty);
            }
            catch { success = false; }

            return Json(new { success = success });
        }
    }
}
