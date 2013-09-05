namespace SaveASpot.Core
{
	public sealed class TypeElementIdentityConverter : IElementIdentityConverter
	{
		public IElementIdentity ToIdentity(string identity)
		{
			return new NullElementIdentity();
		}

		public IElementIdentity ToIdentity(object identity)
		{
			return new NullElementIdentity();
		}

		public IElementIdentity GenerateNew()
		{
			return new NullElementIdentity();
		}

		public bool IsEqual(IElementIdentity left, IElementIdentity right)
		{
			return left.GetType() == right.GetType();
		}
	}
}