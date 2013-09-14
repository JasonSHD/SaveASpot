using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SaveASpot.ViewModels
{
	public sealed class CreateCustomerViewModel
	{
		[Required]
		[Display(Name = Constants.Display.Username)]
		public string UserName { get; set; }

		[Required]
		[Display(Name = Constants.Display.CustomerName)]
		public string CustomerName { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		[Display(Name = Constants.Display.Email)]
		//[RegularExpression(Constants.EmailRegularExpression, ErrorMessage = Constants.Errors.InvalidUserEmail)]
		public string Email { get; set; }

		[Required]
		[StringLength(Constants.PasswordMaxLength, ErrorMessage = Constants.Display.PasswordStringLengthErrorMessage, MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = Constants.Display.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = Constants.Display.ConfirmPassword)]
		[Compare("Password", ErrorMessage = Constants.Display.ConfirmPasswordErrorMessage)]
		public string ConfirmPassword { get; set; }
	}
}