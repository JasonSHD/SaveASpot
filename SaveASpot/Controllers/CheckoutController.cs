using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Controllers
{
	[CustomerAuthorize]
	[AnonymAuthorize]
	public sealed class CheckoutController : BaseController
	{
		[HttpGet]
		public ViewResult Index()
		{
			return View();
		}
	}
}