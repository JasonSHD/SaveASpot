using System.Collections.Generic;
using System.Web;
using SaveASpot.Core;
using SaveASpot.ViewModels.PhasesAndParcels;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IUploadPhasesAndParcelsControllerService
	{
		IMethodResult<IEnumerable<string>> AddParcels(UploadParcelsAndPhasesCollectionViewModel parcelsData);
		IMethodResult<IEnumerable<string>> AddSpots(IEnumerable<HttpPostedFileBase> spotsFiles);
	}
}