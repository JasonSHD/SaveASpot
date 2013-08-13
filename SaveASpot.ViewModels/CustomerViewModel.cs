using SaveASpot.Core;

namespace SaveASpot.ViewModels
{
	public sealed class CustomerViewModel
	{
		public IElementIdentity Identity { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
	}
}