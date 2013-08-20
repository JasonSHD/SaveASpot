using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public sealed class CreateUserResult
	{
		public string MessageKet { get; set; }
		public IElementIdentity UserId { get; set; }
	}
}