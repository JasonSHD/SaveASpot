using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveASpot.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            return View();
        }
			public ActionResult Success()
			{
				return View();
			}

    }
}
