using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Controllers
{
    [AdministratorAuthorize]
    [PhasePageTab(Alias = SiteConstants.UploadPhasesAndParcelsControllerAlias, IndexOfOrder = 40, Title = "UploadPhasesAndParcelsAccordionGroupTitle")]
    public sealed class UploadPhasesAndParcelsController : TabController
    {
        private readonly IArcgisService _arcgisService;
        private readonly ILogger _logger;

        public UploadPhasesAndParcelsController(IArcgisService arcgisService, ILogger logger)
        {
            try
            {
                if (ReferenceEquals(null, arcgisService)) throw new ArgumentNullException("IArcgisService is null");
                if (ReferenceEquals(null, logger)) throw new ArgumentNullException("ILogger is null");

                _logger = logger;
                _arcgisService = arcgisService;
            }
            catch (Exception ex)
            {
                if (_logger != null)
                {
                    _logger.Error(ex.Message, ex);
                }
                else throw;
            }
        }

        public ViewResult Index()
        {
            return TabView(new { });
        }

        [HttpPost]
        public ActionResult UploadSpots(IEnumerable<HttpPostedFileBase> postedSpots)
        {
            var result = new List<string>();

            foreach (var file in postedSpots)
            {
                if (file != null && file.ContentLength > 0)
                {
                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                    string spotsResult = System.Text.Encoding.UTF8.GetString(binData);

                    IMethodResult<MessageResult> resultStatus = _arcgisService.AddSpots(spotsResult);

                    result.Add(file.FileName);

                    //if file wasn't writed log message about it
                    if (!resultStatus.IsSuccess && _logger != null)
                    {
                        result.Add(file.FileName);
                        _logger.Info(string.Format("{0} FileName: {1}", resultStatus.Status.Message, file.FileName));
                    }
                }
            }

            return Json(new { status = result.Count == 0, files = result.ToArray() });
        }

        [HttpPost]
        public JsonResult UploadParcels(IEnumerable<HttpPostedFileBase> postedParcels)
        {
            var result = new List<string>();

            foreach (var file in postedParcels)
            {
                if (file != null && file.ContentLength > 0)
                {
                    BinaryReader b = new BinaryReader(file.InputStream);
                    byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                    string parcelsResult = System.Text.Encoding.UTF8.GetString(binData);

                    IMethodResult<MessageResult> resultStatus = _arcgisService.AddParcels(parcelsResult);

                    //if file wasn't writed log message about it
                    if (!resultStatus.IsSuccess && _logger != null)
                    {
                         result.Add(file.FileName);
                        _logger.Info(string.Format("{0} FileName: {1}", resultStatus.Status.Message, file.FileName));
                    }
                }
            }

            return Json(new { status = result.Count == 0, files = result.ToArray() });
        }
    }
}