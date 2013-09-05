using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class SiteCustomer
	{
		public ObjectId Id { get; set; }
		public ObjectId UserId { get; set; }
		public string StripeUserId { get; set; }
	}
}