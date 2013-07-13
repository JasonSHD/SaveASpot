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

		public void Log(string level, string message)
		{
			var logEntity = new LogEntity() { Id = ObjectId.GenerateNewId(), LogLevel = level, Message = message };
			_mongoDbCollectionFactory.Collection<LogEntity>().Insert(logEntity);
		}

		public void Log(string level, Exception exception)
		{
			var innerEx = exception.InnerException == null ? "" : exception.InnerException.ToString();

			var logEntity = new LogEntity() { Id = ObjectId.GenerateNewId(), LogLevel = level, Message = exception.Message, StackTrace = exception.StackTrace, InnerException = innerEx };
			_mongoDbCollectionFactory.Collection<LogEntity>().Insert(logEntity);
		}
	}
}
