using MongoDB.Bson;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class CustomerRepository : ICustomerRepository
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public CustomerRepository(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public bool AddSpot(IElementIdentity customerId, IElementIdentity spotId)
		{
			return true;
		}

		public bool CreateCustomer(IElementIdentity userIdentity)
		{
			var newCustomer = new SiteCustomer
													{
														Cart = new Cart(),
														Id = ObjectId.GenerateNewId(),
														UserId = userIdentity.ToIdentity()
													};

			_mongoDBCollectionFactory.Collection<SiteCustomer>().Insert(newCustomer);
			return true;
		}
	}
}