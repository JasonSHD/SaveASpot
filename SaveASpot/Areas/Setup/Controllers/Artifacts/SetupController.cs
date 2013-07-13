using System.Web.Mvc;
using SaveASpot.Core.Web.Mvc;

namespace SaveASpot.Areas.Setup.Controllers.Artifacts
{
	public abstract class SetupController : BaseController
	{
		private readonly ISetupConfiguration _setupConfiguration;

		protected SetupController(ISetupConfiguration setupConfiguration)
		{
			_setupConfiguration = setupConfiguration;
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			if (!_setupConfiguration.IsEnabled)
			{
				filterContext.Result = new EmptyResult();
				return;
			}

			base.OnActionExecuting(filterContext);
		}
	}
}