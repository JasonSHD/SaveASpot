using System;
using SaveASpot.Repositories.Interfaces.Logging;
using SaveASpot.Repositories.Models;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Implementations.Logging
{
	public class LogAppender : ILogAppender
	{
		private readonly IMongoDBCollectionFactory _mongoDbCollectionFactory;

		public LogAppender(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDbCollectionFactory = mongoDBCollectionFactory;
		}

		public void Log(string level, string message, Exception exception)
		{
			var realerror = exception;
			var exceptionEntity = new ExceptionData {Message = realerror.Message, StackTrace = realerror.StackTrace};
			var exceptionForDb = exceptionEntity;

			while (realerror.InnerException != null)
			{
				realerror = realerror.InnerException;
				exceptionEntity.InnerException = new ExceptionData
					{
						Message = realerror.Message,
						StackTrace = realerror.StackTrace
					};
				exceptionEntity = exceptionEntity.InnerException;
			}

			var logEntity = new Log()
				{
					Id = ObjectId.GenerateNewId(),
					LogLevel = level,
					Message = message,
					Time = DateTime.Now,
					Exception = exceptionForDb
				};

			_mongoDbCollectionFactory.Collection<Log>().Insert(logEntity);
		}
	}
}
