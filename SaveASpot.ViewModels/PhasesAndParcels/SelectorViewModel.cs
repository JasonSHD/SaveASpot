using System.ComponentModel.DataAnnotations;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class SelectorViewModel
	{
		private const int MinElementsPerPage = 1;
		private const int MaxElementsPerPage = 25;
		private const int DefaultPageNumber = 1;

		[Range(DefaultPageNumber, double.MaxValue)]
		public int PageNumber { get; set; }
		[Range(MinElementsPerPage, MaxElementsPerPage)]
		public int ElementsPerPage { get; set; }

		public string Search { get; set; }

		public int PageCount { get; set; }

		public void Erase()
		{
			PageNumber = DefaultPageNumber;
			ElementsPerPage = MaxElementsPerPage;
			Search = string.Empty;
		}
	}
}