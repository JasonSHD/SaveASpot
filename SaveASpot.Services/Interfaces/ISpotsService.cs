using System.IO;
using SaveASpot.Core;

namespace SaveASpot.Services.Interfaces
{
	public interface ISpotsService
	{
		IMethodResult<MessageResult> AddSpots(StreamReader input);
	}
}