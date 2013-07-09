using System.Web.Mvc;
using System.Web.Routing;
using Ninject;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ControllerFactory : DefaultControllerFactory
	{
		private readonly IKernel _kernel;

		public ControllerFactory(IKernel kernel)
		{
			_kernel = kernel;
		}

		protected override IController GetControllerInstance(RequestContext requestContext, System.Type controllerType)
		{
			if (controllerType == null) return null;

			return (IController)_kernel.Get(controllerType);
		}
	}
}
