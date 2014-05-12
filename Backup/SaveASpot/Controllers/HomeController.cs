using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveASpot.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index(string sponsorID)
        {
            Session["Cart"] = null;
            ViewBag.sponsorID = sponsorID;

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                // redirect to the sponsor checkout.
                return RedirectToAction("Checkout", new { id = id, area = "Settings", controller = "Sponsor" });
            }

            return View();
        }
    }
}
