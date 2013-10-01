using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Data.Models;
using MongoDB.Bson;

namespace SaveASpot.Areas.API.Controllers
{
    public class CartController : SaveASpot.Controllers.ApplicationController
    {
        public JsonResult Add(ObjectId id, ObjectId phaseID)
        {
            bool success = true;
            try
            {
                var phase = Context.Phases.GetPhase(phaseID);
                CurrentCart.Items.Add(new CartItem() { Price = phase.SpotPrice, Phase = phase.PhaseName, SpotID = id });
            }
            catch { success = false; }

            return Json(new { success = success });
        }


        public JsonResult Remove(ObjectId id)
        {
            bool success = false;
            var items = CurrentCart.Items;
            foreach (var item in items)
            {
                if (item.SpotID == id)
                {
                    items.Remove(item);
                    success = true;
                    break;
                }
            }

            return Json(new { success = success });
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
