namespace SaveASpot.Core.Cart
{
	public sealed class SpotElement
	{
		private readonly IElementIdentity _identity;
		private readonly PhaseElement _phaseElement;
		public IElementIdentity Identity { get { return _identity; } }
		public PhaseElement PhaseElement { get { return _phaseElement; } }

		public SpotElement(IElementIdentity identity, PhaseElement phaseElement)
		{
			_identity = identity;
			_phaseElement = phaseElement;
		}
	}
}