namespace SaveASpot.Core
{
	public sealed class MessageMethodResult : IMethodResult<MessageResult>
	{
		private readonly bool _isSuccess;
		private readonly MessageResult _messageStatus;
		public bool IsSuccess { get { return _isSuccess; } }
		public MessageResult Status { get { return _messageStatus; } }

		public MessageMethodResult(bool isSuccess, string message)
		{
			_isSuccess = isSuccess;
			_messageStatus = new MessageResult(message);
		}
	}
}