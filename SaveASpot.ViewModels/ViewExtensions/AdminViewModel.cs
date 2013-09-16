using SaveASpot.Core.Security;

namespace SaveASpot.ViewModels.ViewExtensions
{
	public sealed class AdminViewModel
	{
		public User User { get; set; }
		public User Anonym { get; set; }
	}
}