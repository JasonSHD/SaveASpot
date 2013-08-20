namespace SaveASpot.Services.Interfaces
{
	public interface ITypeConverter<in TSource, out TViewModel>
	{
		TViewModel Convert(TSource source);
	}
}