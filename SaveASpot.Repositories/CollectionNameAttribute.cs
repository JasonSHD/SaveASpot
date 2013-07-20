using System;

namespace SaveASpot.Repositories
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class CollectionNameAttribute : Attribute
	{
		private readonly string _collectionName;
		public string CollectionName { get { return _collectionName; } }

		public CollectionNameAttribute(string collectionName)
		{
			_collectionName = collectionName;
		}
	}
}
