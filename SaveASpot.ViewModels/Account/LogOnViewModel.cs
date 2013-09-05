using System.ComponentModel.DataAnnotations;

namespace SaveASpot.ViewModels.Account
{
	public sealed class LogOnViewModel
	{
		[Required]
		[Display(Name = Constants.Display.Username)]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Display(Name = Constants.Display.Password)]
		public string Password { get; set; }

		[Display(Name = Constants.Display.RememberMe)]
		public bool RememberMe { get; set; }
	}
}
