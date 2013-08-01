﻿using System.Collections.Generic;
using System.IO;
using System.Web;
using SaveASpot.Core;
using SaveASpot.Core.Logging;
using SaveASpot.Services.Interfaces;
using SaveASpot.Services.Interfaces.Controllers;

namespace SaveASpot.Services.Implementations.Controllers
{
	public sealed class UploadPhasesAndParcelsControllerService : IUploadPhasesAndParcelsControllerService
	{
		private readonly ISpotsService _spotsService;
		private readonly IParcelsService _parcelsService;
		private readonly ILogger _logger;
		private readonly ITextService _textService;

		public UploadPhasesAndParcelsControllerService(ISpotsService spotsService, IParcelsService parcelsService, ILogger logger, ITextService textService)
		{
			_spotsService = spotsService;
			_parcelsService = parcelsService;
			_logger = logger;
			_textService = textService;
		}

		public IMethodResult<IEnumerable<string>> AddParcels(IEnumerable<HttpPostedFileBase> parcelsFiles)
		{
			var result = new List<string>();

			foreach (var file in parcelsFiles)
			{
				if (file != null && file.ContentLength > 0)
				{
					var b = new BinaryReader(file.InputStream);
					byte[] binData = b.ReadBytes((int)file.InputStream.Length);

					IMethodResult<MessageResult> resultStatus = _parcelsService.AddParcels(new StreamReader(new MemoryStream(binData)));

					//if file wasn't writed log message about it
					if (!resultStatus.IsSuccess)
					{
						result.Add(file.FileName);
						_logger.Info(string.Format("{0} FileName: {1}", _textService.ResolveTest(resultStatus.Status.Message), file.FileName));
					}
				}
			}

			return new MethodResult<IEnumerable<string>>(result.Count < 1, result);
		}

		public IMethodResult<IEnumerable<string>> AddSpots(IEnumerable<HttpPostedFileBase> spotsFiles)
		{
			var result = new List<string>();

			foreach (var file in spotsFiles)
			{
				if (file != null && file.ContentLength > 0)
				{
					var b = new BinaryReader(file.InputStream);
					byte[] binData = b.ReadBytes((int)file.InputStream.Length);
					IMethodResult<MessageResult> resultStatus = _spotsService.AddSpots(new StreamReader(new MemoryStream(binData)));

					//if file wasn't writed log message about it
					if (!resultStatus.IsSuccess)
					{
						result.Add(file.FileName);
						_logger.Info(string.Format("{0} FileName: {1}", _textService.ResolveTest(resultStatus.Status.Message), file.FileName));
					}
				}
			}

			return new MethodResult<IEnumerable<string>>(result.Count < 1, result);
		}
	}
}