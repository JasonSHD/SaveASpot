using System.Collections.Generic;
using System.Linq;

namespace SaveASpot.Core.Validation
{
	public sealed class NotValidatorBuilder : IValidator
	{
		private readonly IList<IValidator> _validators = new List<IValidator>();

		public NotValidatorBuilder Add(IValidator validator)
		{
			_validators.Add(validator);
			return this;
		}

		public IValidationResult Validate(object input)
		{
			if (_validators.Any(validator => validator.Validate(input).IsValid))
			{
				return new ValidationResult(false, string.Empty);
			}

			return new ValidationResult(true, string.Empty);
		}
	}
}