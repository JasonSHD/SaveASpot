using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class SiteCustomer
	{
		public ObjectId Id { get; set; }
		public string Identity { get { return Id.ToString(); } }
		public ObjectId UserId { get; set; }
	}
}