using MongoDB.Bson;

namespace SaveASpot.Repositories.Models.Security
{
	public sealed class UserEntity
	{
		public ObjectId Id { get; set; }
		public string Identity { get { return Id.ToString(); } }
		public string Username { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string[] Roles { get; set; }
	}
}
