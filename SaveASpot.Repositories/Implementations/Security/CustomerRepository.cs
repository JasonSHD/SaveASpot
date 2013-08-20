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
														UserId = userIdentity.ToIdentity()
													};

			_mongoDBCollectionFactory.Collection<SiteCustomer>().Insert(newCustomer);
			return true;
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
	}
}