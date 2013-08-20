using System;

namespace SaveASpot.Core.Validation
{
	public sealed class EntityValidatorBuilder<T> : IValidator
	{
		public IValidator Validator { get; private set; }

		public EntityValidatorBuilder()
		{
			Validator = new NullValidator();
		}

		public EntityValidatorBuilder<T> And(IValidator validator)
		{
			Validator = Validator.And(validator);

			return this;
		}

		public EntityValidatorBuilder<T> For<TResul>(Func<T, TResul> accessor, Func<IValidator, IValidator> builder)
		{
			Validator = Validator.And(new PropertyValidator<T>(e => accessor(e), builder));

			return this;
		}

		public IValidationResult Validate(object input)
		{
			return Validator.Validate(input);
		}

		private sealed class PropertyValidator<T> : IValidator
		{
			private readonly Func<T, object> _accessor;
			private readonly Func<IValidator, IValidator> _builder;

			public PropertyValidator(Func<T, object> accessor, Func<IValidator, IValidator> builder)
			{
				_accessor = accessor;
				_builder = builder;
			}

			public IValidationResult Validate(object input)
			{
				if (!(input is T))
				{
					return new ValidationResult(false, "InvalidCast");
				}

				var propertyValue = _accessor((T)input);
				var validator = _builder(new NullValidator());

				return validator.Validate(propertyValue);
			}
		}
	}
}