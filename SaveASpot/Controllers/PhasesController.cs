using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.ViewModels;

namespace SaveASpot.Controllers

{
	[TabDescriptions(SiteConstants.PhasesControllerAlias, "Phases")]
	[AdministratorAuthorize]
	public sealed class PhasesController : AdminTabController
	{
		public ActionResult Index()
		{
			return TabView(new { });
		}

        [HttpPost]
        public ActionResult UploadPhases(IEnumerable<HttpPostedFileBase> postedPhases)
        {
            foreach (var file in postedPhases)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    //file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UploadParcels(IEnumerable<HttpPostedFileBase> postedParcels)
        {
            foreach (var file in postedParcels)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    //file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }
	}
}