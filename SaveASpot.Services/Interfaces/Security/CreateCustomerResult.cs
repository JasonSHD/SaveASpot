using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public sealed class CreateCustomerResult
	{
		public IElementIdentity UserId { get; set; }
		public IElementIdentity CustomerId { get; set; }
		public string MessageKey { get; set; }
	}
}