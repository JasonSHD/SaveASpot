using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Core;
using SaveASpot.Repositories.Interfaces.Security;
using SaveASpot.Repositories.Models.Security;

namespace SaveASpot.Repositories.Implementations.Security
{
	public sealed class CustomerQueryable : BasicMongoDBElementQueryable<SiteCustomer, ICustomerFilter>, ICustomerQueryable
	{
		public CustomerQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory)
			: base(mongoDBCollectionFactory)
		{
		}

		public ICustomerFilter FilterByUserId(IElementIdentity identity)
		{
			var id = identity.ToIdentity();

			return new CustomerFilter(Query<SiteCustomer>.Where(e => e.UserId == id));
		}

		public ICustomerFilter All()
		{
			return new CustomerFilter(Query<SiteCustomer>.Where(e => true));
		}

		protected override ICustomerFilter BuildFilter(IMongoQuery query)
		{
			return new CustomerFilter(query);
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