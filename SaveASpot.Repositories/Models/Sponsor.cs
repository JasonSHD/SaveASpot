using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public sealed class Sponsor
	{
		public ObjectId Id { get; set; }
		public string Identity { get { return Id.ToString(); } }
		public string CompanyName { get; set; }
		public string Sentence { get; set; }
		public string Url { get; set; }
		public string Logo { get; set; }
	}
}
