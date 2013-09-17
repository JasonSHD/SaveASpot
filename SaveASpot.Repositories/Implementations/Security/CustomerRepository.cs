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

		public bool UpdateSiteCustomer(IElementIdentity id, string stripeUserToken)
		{
			var customerId = id.ToIdentity();

			var result = _mongoDBCollectionFactory.Collection<SiteCustomer>()
															 .Update(Query<SiteCustomer>.Where(e => e.Id == customerId),
																			 Update<SiteCustomer>.Set(e => e.StripeUserId, stripeUserToken));

			return result.DocumentsAffected == 1;
		}
	}
}