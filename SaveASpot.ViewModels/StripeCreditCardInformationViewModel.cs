using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace SaveASpot.ViewModels
{
	public sealed class StripeCreditCardInformationViewModel
	{
		[Required]
		[Display(Name = "Name on Card")]
		public string NameOnCard { get; set; }

		[Required]
		[Display(Name = "Card Number")]
		public string CardNumber { get; set; }

		[Required]
		public string ExpirationMonth { get; set; }

		[Required]
		public string ExpirationYear { get; set; }

		[Required]
		[StringLength(4)]
		public int Cvc { get; set; }

		[Display(Name = "Postal Code")]
		public int PostalCode { get; set; }
		
	}
}
