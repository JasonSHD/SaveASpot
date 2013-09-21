using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface IParcelsService
	{
		IMethodResult<MessageResult> AddParcels(StreamReader input, decimal spotPrice);
	}
}