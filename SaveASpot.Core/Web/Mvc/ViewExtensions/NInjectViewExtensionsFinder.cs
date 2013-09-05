using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;

namespace SaveASpot.Core.Web.Mvc.ViewExtensions
{
	public sealed class NInjectViewExtensionsFinder : IViewExtensionsFinder
	{
		private readonly IKernel _kernel;
		private readonly Assembly _viewExtensionsAssembly;

		public NInjectViewExtensionsFinder(IKernel kernel, Assembly viewExtensionsAssembly)
		{
			_kernel = kernel;
			_viewExtensionsAssembly = viewExtensionsAssembly;
		}

		public IEnumerable<IViewExtension> FindViewExtensions()
		{
			return
				_viewExtensionsAssembly.GetTypes()
				                       .Where(e => e.GetInterfaces().Any(intType => intType == typeof(IViewExtension)))
				                       .Select(e => _kernel.Get(e))
				                       .OfType<IViewExtension>();
		}
	}
}