namespace SaveASpot.Repositories.Models
{
	public class ExceptionData
	{
		public string Message { get; set; }
		public string StackTrace { get; set; }
		public ExceptionData InnerException { get; set; }
	}
}
