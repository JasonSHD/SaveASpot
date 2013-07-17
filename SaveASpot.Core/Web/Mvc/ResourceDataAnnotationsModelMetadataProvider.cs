using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SaveASpot.Core.Web.Mvc
{
	public sealed class ResourceDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
	{
		private readonly ITextService _textService;

		public ResourceDataAnnotationsModelMetadataProvider(ITextService textService)
		{
			_textService = textService;
		}

		protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
		{
			var result = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

			var displayName = _textService.ResolveTest(result.DisplayName);
			if (!string.IsNullOrWhiteSpace(displayName))
			{
				result.DisplayName = displayName;
			}

			return result;
		}
	}
}