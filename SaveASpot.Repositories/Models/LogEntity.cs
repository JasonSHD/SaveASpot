using System;
using MongoDB.Bson;

namespace SaveASpot.Repositories.Models
{
	public class LogEntity
	{
		public ObjectId Id { get; set; }
		public string Message { get; set; }
		public string LogLevel { get; set; }
		public string StackTrace { get; set; }
		public string InnerException { get; set; }
		public DateTime Time { get; set; }
	}
}
