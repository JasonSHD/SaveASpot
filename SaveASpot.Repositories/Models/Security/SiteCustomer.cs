using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class SiteCustomer
	{
		public SiteCustomer()
		{
			Cart = new Cart();
		}

		public ObjectId Id { get; set; }
		public string Identity { get { return Id.ToString(); } }
		public ObjectId UserId { get; set; }
		public Cart Cart { get; set; }
	}
}