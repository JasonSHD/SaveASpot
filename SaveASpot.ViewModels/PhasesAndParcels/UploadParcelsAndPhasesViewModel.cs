using System.Web;

namespace SaveASpot.ViewModels.PhasesAndParcels
{
	public sealed class UploadParcelsAndPhasesViewModel
	{
		public HttpPostedFileBase ParcelFile { get; set; }
		public decimal SpotPrice { get; set; }
	}

	public sealed class UploadParcelsAndPhasesCollectionViewModel
	{
		public UploadParcelsAndPhasesViewModel[] ParcelsAndPhases { get; set; }
	}
}