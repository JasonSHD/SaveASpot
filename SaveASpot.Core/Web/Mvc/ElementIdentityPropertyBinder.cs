using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ElementIdentityPropertyBinder : DefaultModelBinder
	{
		private readonly IElementIdentityConverter _elementIdentityConverter;

		public ElementIdentityPropertyBinder(IElementIdentityConverter elementIdentityConverter)
		{
			_elementIdentityConverter = elementIdentityConverter;
		}

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			if (bindingContext.ModelType == typeof(IElementIdentity))
			{
				return
					_elementIdentityConverter.ToIdentity(bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue);
			}

			return base.BindModel(controllerContext, bindingContext);
		}
	}
}