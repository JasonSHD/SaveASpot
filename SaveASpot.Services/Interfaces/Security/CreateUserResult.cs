using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Security
{
	public sealed class CreateUserResult
	{
		public string MessageKey { get; set; }
		public IElementIdentity UserId { get; set; }
	}
}