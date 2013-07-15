using System;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public class Log
	{
		public ObjectId Id { get; set; }
		public string Message { get; set; }
		public string LogLevel { get; set; }
		public ExceptionData Exception { get; set; }
		public DateTime Time { get; set; }
	}
}
