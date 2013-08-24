using System.Linq;
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

		public bool CreateCustomer(IElementIdentity userIdentity)
		{
			var newCustomer = new SiteCustomer
													{
														Cart = new Cart(),
														Id = ObjectId.GenerateNewId(),
														UserId = userIdentity.ToIdentity(),
														StripeUserId = string.Empty
													};

			_mongoDBCollectionFactory.Collection<SiteCustomer>().Insert(newCustomer);
			return true;
		}

		public SiteCustomer GetCustomerById(string id)
		{
			var customerId = id.ToIdentity();

			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.Id == customerId));

			return customer;
		}

		public bool AddSpotToCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity)
		{
			var customerId = customerIdentity.ToIdentity();
			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.Id == customerId));

			if (customer == null) return false;

			customer.Cart.SpotIdCollection = customer.Cart.SpotIdCollection.Union(new[] { spotIdentity.ToIdentity() }).ToArray();
			_mongoDBCollectionFactory.Collection<SiteCustomer>().Save(customer);

			return true;
		}

		public bool RemoveSpotFromCart(IElementIdentity customerIdentity, IElementIdentity spotIdentity)
		{
			var customerId = customerIdentity.ToIdentity();
			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.Id == customerId));

			if (customer == null) return false;

			var identities = customer.Cart.SpotIdCollection.ToList();
			identities.Remove(spotIdentity.ToIdentity());
			customer.Cart.SpotIdCollection = identities.ToArray();
			_mongoDBCollectionFactory.Collection<SiteCustomer>().Save(customer);

			return true;
		}

		public bool UpdateCustomerCart(string customerId, ObjectId[] cartIdentity)
		{
			var custId = customerId.ToIdentity();

			var customer =
				_mongoDBCollectionFactory.Collection<SiteCustomer>().FindOne(Query<SiteCustomer>.Where(e => e.Id == custId));

			customer.Cart.SpotIdCollection = cartIdentity;
			var result = _mongoDBCollectionFactory.Collection<SiteCustomer>().Save(customer);

			return result.DocumentsAffected == 1;

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