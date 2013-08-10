namespace SaveASpot.Services.Interfaces.Security
{
	public sealed class CreateCustomerResult
	{
		public string UserId { get; set; }
		public string CustomerId { get; set; }
		public string MessageKey { get; set; }
	}
}