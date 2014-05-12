using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SaveASpot.Models
{
    public class AllowCrossSiteJsonAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string allow = "*";
            /*string allow = "http://www.saveaspot.org";

            if (filterContext.RequestContext.HttpContext.Request.UrlReferrer.OriginalString.Contains("saveaspot.squarehook.com"))
            {
                allow = "http://saveaspot.squarehook.com";
            }*/
            filterContext.RequestContext.HttpContext.Response.AddHeader("Access-Control-Allow-Origin", allow);
            base.OnActionExecuting(filterContext);
        }
    }
}