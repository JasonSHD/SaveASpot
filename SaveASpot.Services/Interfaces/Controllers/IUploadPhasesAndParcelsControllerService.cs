using System.Collections.Generic;
using System.Web;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces.Controllers
{
	public interface IUploadPhasesAndParcelsControllerService
	{
		IMethodResult<IEnumerable<string>> AddParcels(IEnumerable<HttpPostedFileBase> parcelsFiles);
		IMethodResult<IEnumerable<string>> AddSpots(IEnumerable<HttpPostedFileBase> spotsFiles);
	}
}