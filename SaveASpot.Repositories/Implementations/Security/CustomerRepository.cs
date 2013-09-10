using MongoDB.Bson;
using MongoDB.Driver.Builders;
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

		public IElementIdentity CreateCustomer(IElementIdentity userIdentity)
		{
			var newCustomer = new SiteCustomer
													{
														Id = ObjectId.GenerateNewId(),
														UserId = userIdentity.ToIdentity(),
														StripeUserId = string.Empty
													};

			_mongoDBCollectionFactory.Collection<SiteCustomer>().Insert(newCustomer);
			return new MongoDBIdentity(newCustomer.Id);
		}

		public SiteCustomer GetCustomerById(string id)
		{
			var customerId = id.ToIdentity();

			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.Id == customerId));

			return customer;
		}

		public bool UpdateSiteCustomer(string id, string stripeUserToken)
		{
			var customerId = id.ToIdentity();

			var result = _mongoDBCollectionFactory.Collection<SiteCustomer>()
															 .Update(Query<SiteCustomer>.EQ(e => e.UserId, customerId),
																			 Update<SiteCustomer>.Set(e => e.StripeUserId, stripeUserToken));

			return result.DocumentsAffected == 1;
		}

		public SiteCustomer GetCustomerByUserId(string userId)
		{
			var customerId = userId.ToIdentity();

			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.UserId == customerId));

			return customer;
		}

	}
}