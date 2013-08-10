using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using SaveASpot.Repositories.Interfaces;

namespace SaveASpot.Repositories.Implementations
{
	public abstract class BasicMongoDBElementQueryable<T, TFilter> : IElementQueryable<T, TFilter>
	{
		private readonly IMongoDBCollectionFactory _mongoDBCollectionFactory;

		protected BasicMongoDBElementQueryable(IMongoDBCollectionFactory mongoDBCollectionFactory)
		{
			_mongoDBCollectionFactory = mongoDBCollectionFactory;
		}

		public TFilter And(TFilter left, TFilter right)
		{
			var leftQuery = MongoQueryFilter.Convert(left);
			var rightQuery = MongoQueryFilter.Convert(right);

			return BuildFilter(Query.And(leftQuery.MongoQuery, rightQuery.MongoQuery));
		}

		public IEnumerable<T> Find(TFilter filter)
		{
			return _mongoDBCollectionFactory.Collection<T>().Find(MongoQueryFilter.Convert(filter).MongoQuery);
		}

		protected abstract TFilter BuildFilter(IMongoQuery query);
	}
}