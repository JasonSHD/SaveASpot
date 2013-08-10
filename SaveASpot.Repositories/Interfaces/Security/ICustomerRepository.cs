namespace SaveASpot.Repositories.Interfaces.Security
{
	public interface ICustomerRepository
	{
		bool AddSpot(string customerId, string spotId);
		bool CreateCustomer(string userIdentity);
	}
}