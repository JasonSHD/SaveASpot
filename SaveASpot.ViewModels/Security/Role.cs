namespace SaveASpot.ViewModels.Security
{
	public abstract class Role
	{
		private readonly string _identity;
		private readonly string _name;
		public string Name { get { return _name; } }
		public string Identity { get { return _identity; } }

		protected Role(string identity, string name)
		{
			_identity = identity;
			_name = name;
		}

		#region Sealed members
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
		#endregion Sealed members

		public static bool operator ==(Role left, Role right)
		{
			if (ReferenceEquals(null, left) && ReferenceEquals(null, right)) return true;
			if (ReferenceEquals(null, left) || ReferenceEquals(null, right)) return false;

			return left.Identity == right.Identity;
		}

		public static bool operator !=(Role left, Role right)
		{
			return !(left == right);
		}
	}
}