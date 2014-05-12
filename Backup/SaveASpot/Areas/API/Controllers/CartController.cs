using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Data.Models;
using MongoDB.Bson;
using SaveASpot.Models;

namespace SaveASpot.Areas.API.Controllers
{
    public class CartController : SaveASpot.Controllers.ApplicationController
    {
        public JsonResult Add(ObjectId id, ObjectId phaseID)
        {
            bool success = true;
            try
            {
                if (phaseID == ObjectId.Empty)
                {
                    var spot = Context.Spots.GetSpot(id);
                    phaseID = spot.PhaseID;
                }
                var phase = Context.Phases.GetPhase(phaseID);
                var item = new CartItem() { Price = phase.SpotPrice, SponsorPrice = phase.SponsorPrice, Phase = phase.PhaseName, SpotID = id };
                var cart = CurrentCart;
                cart.Items.Add(item);
                Context.Carts.AddItem(ClientIP, item, ref cart);

            }
            catch { success = false; }

            return Json(new { success = success });
        }


        public JsonResult Remove(ObjectId id)
        {
            bool success = false;
            var cart = CurrentCart;
            var items = cart.Items;
            foreach (var item in items)
            {
                if (item.SpotID == id)
                {
                    item.Qty = 0;
                    Context.Carts.AddItem(ClientIP, item, ref cart);
                    success = true;
                    break;
                }
            }

            return Json(new { success = success, id = id.ToString() });
        }

        [AllowCrossSiteJson]
        [HttpPost]
        public JsonResult AddItems(ObjectId id, int qty, ObjectId sponsorID)
        {
            var cart = Context.Carts.GetCart(ObjectId.Empty, ClientIP) ??
                new Cart { CartID = ObjectId.GenerateNewId(), ClientIP = ClientIP, Items = new List<CartItem>() };

            bool success = true;
            string message = "";
            int totalQty = 0;
            double totalPrice = 0.0;
            try
            {
                if (id == ObjectId.Empty)
                {
                    var active = Context.Phases.GetActivePhase();
                    id = active.PhaseID;
                }
                var phase = Context.Phases.GetPhase(id);
                var item = new CartItem() { Price = phase.SpotPrice, SponsorPrice = phase.SponsorPrice, Phase = phase.PhaseName, SpotID = ObjectId.Empty, Qty = qty, PhaseID = id };

                var sponsor = Context.Sponsors.GetSponsor(sponsorID);
                if (sponsor != null)
                {
                    item.SponsorID = sponsorID;
                    item.Sponsor = sponsor.Name;
                }

                success = Context.Carts.AddItem(ClientIP, item, ref cart);

                totalQty = cart.Items.Sum(i => i.Qty);
                totalPrice = cart.Items.Sum(i => i.Qty * i.Price);
            }
            catch (Exception exp) { success = false; message = exp.Message; }

            return Json(new { success = success, quantity = totalQty, total = totalPrice });
        }

        [AllowCrossSiteJson]
        [HttpPost]
        public JsonResult UpdateItems(ObjectId id, int qty)
        {
            return AddItems(id, qty, ObjectId.Empty);
        }

        [AllowCrossSiteJson]
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GetCart()
        {
            bool success = true;
            int totalQty = 0;
            double totalPrice = 0.0;
            
            try 
            {
                var cart = Context.Carts.GetCart(ObjectId.Empty, ClientIP) ??
                    new Cart { CartID = ObjectId.GenerateNewId(), ClientIP = ClientIP, Items = new List<CartItem>() };
                totalQty = cart.Items.Sum(i => i.Qty);
                totalPrice = cart.Items.Sum(i => i.Qty * i.Price);
            }
            catch { success = false; }

            return Json(new { success = success, quantity = totalQty, total = totalPrice });
        }

        [AllowCrossSiteJson]
        public JsonResult GetProjectSponsors(ObjectId id)
        {
            List<Sponsor> sponsors = new List<Sponsor>();
            bool success = true;

            try
            {
                //var sponsorIDs = Context.SponsorSpots.GetAllSponsorIDsByPhase(id);
                //sponsors = Context.Sponsors.GetSponsors(sponsorIDs);
                sponsors = Context.Sponsors.GetActiveSponsors();

                // filter and order appropriately
                sponsors = (from s in sponsors where s.ShowSponsor orderby s.Order select s).ToList();
            }
            catch (Exception exp)
            {
                success = false;
            }

            return Json(new { success = success, results = sponsors }, JsonRequestBehavior.AllowGet);
        }
    }
}
