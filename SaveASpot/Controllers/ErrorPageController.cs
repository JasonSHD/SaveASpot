using System.Web.Mvc;
using SaveASpot.Core.Logging;

namespace SaveASpot.Controllers
{
	public class ErrorPageController : Controller
	{
		private readonly ILogger _logger;

		public ErrorPageController(ILogger logger)
		{
			_logger = logger;
		}

		public ActionResult Index()
		{
			return View("Error");
		}

		[HttpPost]
		public void LogJavascriptError(string message, string source, string lineNumber)
		{
			_logger.JavaScriptError(string.Format("An javascript error <{0}> has occurred in <{1}> on line number <{2}>", message, source, lineNumber));
		}

	}
}
