namespace SaveASpot.Core.Validation
{
	public abstract class BaseValidator<T> : IValidator
	{
		private readonly string _defaultMessage;
		public string Message { get; set; }

		protected BaseValidator(string defaultMessage)
		{
			_defaultMessage = defaultMessage;
		}

		public IValidationResult Validate(object input)
		{
			if (!(input is T)) return new ValidationResult(false, "InvalidObjectType");

			return BuildResult(IsValid((T)input));
		}

		protected virtual IValidationResult BuildResult(bool isValid)
		{
			return new ValidationResult(isValid, isValid ? string.Empty : Message ?? _defaultMessage);
		}

		protected abstract bool IsValid(T input);

		#region sealed members
		public override sealed bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override sealed int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override sealed string ToString()
		{
			return base.ToString();
		}
		#endregion sealed members
	}
}