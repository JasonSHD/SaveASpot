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

        public ActionResult Edit(ObjectId id)
        {
            var model = new SponsorDetail();
            model.Sponsor = Context.Sponsors.GetSponsor(id);

            // get spot information
            model.SpotInfo = new Count();// Context.Spots.GetSpotsInfoByPhase(id);

            // get sponsor information
            model.SponsorInfo = new Count();// Context.SponsorSpots.GetSpotsInfoByPhase(id);

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
            CurrentCart.SponsorID = id;
            return View(CurrentCart);
        }

        [HttpPost]
        public ActionResult Checkout(Cart model)
        {
            var cart = CurrentCart;

            foreach (var item in cart.Items)
            {
                var spot = Context.SponsorSpots.GetSpot(item.SpotID);
                spot.Taken = true;
                spot.SponsorID = model.SponsorID;

                Context.SponsorSpots.SaveSpot(spot);
            }

            return RedirectToAction("Edit", new { id = model.SponsorID });
        }

        private Cart CurrentCart
        {
            get
            {
                object cart = Session["Cart"];
                if (cart == null)
                {
                    cart = new Cart() { CartID = ObjectId.GenerateNewId(), Items = new List<CartItem>() };
                    Session["Cart"] = cart;
                }
                return (Cart)cart;
            }
            set
            {
                Session["Cart"] = value;
            }
        }
    }
}
