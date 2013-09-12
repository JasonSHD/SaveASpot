namespace SaveASpot.Core.Cart
{
	public sealed class PhaseElement
	{
		private readonly IElementIdentity _identity;
		private readonly decimal _price;
		private readonly string _name;
		public IElementIdentity Identity { get { return _identity; } }
		public decimal SpotPrice { get { return _price; } }
		public string Name { get { return _name; } }

		public PhaseElement(IElementIdentity identity, decimal price, string name)
		{
			_identity = identity;
			_price = price;
			_name = name;
		}
	}
}