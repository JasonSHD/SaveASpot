namespace SaveASpot.Core
{
	public sealed class ValidationResult : IValidationResult
	{
		private readonly bool _isValid;
		private readonly string _message;
		public bool IsValid { get { return _isValid; } }
		public string Message { get { return _message; } }

		public ValidationResult(bool isValid, string message)
		{
			_isValid = isValid;
			_message = message;
		}
	}
}