using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveASpot.Controllers
{
    public class ErrorPageController : Controller
    {
        public ActionResult Index()
        {
            return View("Error");
        }

    }
}
