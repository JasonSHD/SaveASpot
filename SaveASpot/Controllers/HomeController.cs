using System;
using System.IO;
using System.Web.Mvc;
using SaveASpot.Core.Logging;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	public class HomeController : BaseController
	{
		//private readonly ILogger _logger;

		//public HomeController(ILogger logger)
		//{
		//	_logger = logger;
		//}

		public ActionResult Index()
		{
			//Exception exception = null;
			//try
			//{
			//	throw new NotImplementedException("Not Implement!", new Exception("My Inner"));
			//}
			//catch (Exception ex)
			//{
			//	exception = ex;
			//}

			//_logger.Log(new ErrorLogEntry(exception, "Test error"));

			ViewBag.Message = "Welcome to ASP.NET MVC!";

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
	}
}
