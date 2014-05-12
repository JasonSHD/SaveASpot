using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveASpot.Controllers
{
    public class CheckoutController : ApplicationController
    {
        //
        // GET: /Cart/

        public ActionResult Index(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                // redirect to the sponsor checkout.
                return RedirectToAction("Checkout", new { id = id, area = "Settings", controller = "Sponsor" });
            }

            var cart = CurrentCart;
            cart.Years = new List<int>();
            int startYear = DateTime.Now.Year;
            int endYear = startYear + 10;

            while (startYear < endYear)
            {
                cart.Years.Add(startYear);
                startYear = startYear + 1;
            }

            ViewBag.StripePublicKey = Configuration.AppSettings["StripePublicKey"];

            return View(CurrentCart);
        }

        [HttpPost]
        public ActionResult Index(dynamic model, string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                // redirect to the sponsor checkout.
                return RedirectToAction("Checkout", new { id = id, area = "Settings", controller = "Sponsor" });
            }

            var cart = CurrentCart;
            cart.Years = new List<int>();
            int startYear = DateTime.Now.Year;
            int endYear = startYear + 10;

            while (startYear < endYear)
            {
                cart.Years.Add(startYear);
                startYear = startYear + 1;
            }

            ViewBag.StripePublicKey = Configuration.AppSettings["StripePublicKey"];

            return View(CurrentCart);
        }
    }
}
