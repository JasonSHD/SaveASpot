using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SaveASpot.ViewModels
{
	public sealed class RegisterViewModel
	{
		[Required]
		[Display(Name = "User name")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

		[Required]
		[StringLength(PasswordMaxLength, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = PasswordMinLength)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public const int PasswordMinLength = 6;
		public const int PasswordMaxLength = 100;
	}
}