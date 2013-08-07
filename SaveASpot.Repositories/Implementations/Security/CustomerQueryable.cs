using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class CustomerQueryable : ICustomerQueryable
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		public CustomerQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public ICustomerFilter FilterByUserId(string identity)
		{
			var id = identity.ToIdentity();

			return new CustomerFilter(Query<SiteCustomer>.Where(e => e.UserId == id));
		}

		public IEnumerable<SiteCustomer> Find(ICustomerFilter customerFilter)
		{
			return _mongoDBCollectionFactory.Collection<SiteCustomer>().Find(MongoQueryFilter.Convert(customerFilter).MongoQuery);
		}

		private sealed class CustomerFilter : MongoQueryFilter, ICustomerFilter
		{
			public CustomerFilter(IMongoQuery mongoQuery)
				: base(mongoQuery)
			{
			}
		}
	}
}