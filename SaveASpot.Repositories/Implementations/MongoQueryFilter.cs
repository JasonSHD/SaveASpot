using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace SaveASpot.Repositories.Implementations
{
	public abstract class MongoQueryFilter
	{
		private readonly IMongoQuery _mongoQuery;
		public IMongoQuery MongoQuery { get { return _mongoQuery; } }

		protected MongoQueryFilter(IMongoQuery mongoQuery)
		{
			_mongoQuery = mongoQuery;
		}

		#region sealed members
		public override sealed bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override sealed int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override sealed string ToString()
		{
			return base.ToString();
		}
		#endregion sealed members

		public static MongoQueryFilter Convert(object filter)
		{
			if (filter is MongoQueryFilter)
			{
				return (MongoQueryFilter)filter;
			}

			return new MongoQueryFilterNull();
		}

		private sealed class MongoQueryFilterNull : MongoQueryFilter
		{
			public MongoQueryFilterNull()
				: base(Query.Null)
			{
			}
		}
	}
}