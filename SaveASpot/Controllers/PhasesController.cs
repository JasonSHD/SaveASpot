using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SaveASpot.Controllers.Artifacts;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Web.Mvc;
using SaveASpot.Services.Interfaces;

namespace SaveASpot.Controllers
{
    [AdministratorAuthorize]
    public sealed class PhasesController : TabController
    {
        private readonly IArcgisService _arcgisService;
        private readonly ILogger _logger;

        public ActionResult Index()
        {
            return TabView(new { });
        }

        public PhasesController(IArcgisService arcgisService, ILogger logger)
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

        [HttpPost]
        public ActionResult UploadSpots(IEnumerable<HttpPostedFileBase> postedSpots)
        {
            foreach (var file in postedSpots)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        BinaryReader b = new BinaryReader(file.InputStream);
                        byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                        string spotsResult = System.Text.Encoding.UTF8.GetString(binData);
                        _arcgisService.AddSpots(spotsResult);
                    }
                    catch (Exception ex)
                    {
                        if (_logger == null) throw;
                        _logger.Error(string.Format("Class: {0}; Method: {1}; ex.message:{2}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);    
                    }
                  
                }
            }
            return RedirectToAction("Index", "Map");
        }

        [HttpPost]
        public ActionResult UploadParcels(IEnumerable<HttpPostedFileBase> postedParcels)
        {
            foreach (var file in postedParcels)
            {
                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        BinaryReader b = new BinaryReader(file.InputStream);
                        byte[] binData = b.ReadBytes((int)file.InputStream.Length);
                        string parcelsResult = System.Text.Encoding.UTF8.GetString(binData);
                        _arcgisService.AddParcels(parcelsResult);
                    }
                    catch (Exception ex)
                    {
                        if (_logger == null) throw;
                        _logger.Error(string.Format("Class: {0}; Method: {1}; ex.message:{2}", this.GetType().FullName, MethodBase.GetCurrentMethod().Name, ex.Message), ex);
                    }
                   
                }
            }
            return RedirectToAction("Index", "Map");
        }
    }
}