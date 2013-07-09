namespace SaveASpot.Core
{
	public sealed class MessageResult
	{
		private readonly string _message;
		public string Message { get { return _message; } }

		public MessageResult(string message)
		{
			_message = message;
		}
	}
}