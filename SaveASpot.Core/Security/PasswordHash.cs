using System.Security.Cryptography;
using System.Text;

namespace SaveASpot.Core.Security
{
	public sealed class PasswordHash : IPasswordHash
	{
		public string GetHash(string password, string username)
		{
			return GetHashString(username + password);
		}

		private static byte[] GetHash(string inputString)
		{
			HashAlgorithm algorithm = MD5.Create();  // SHA1.Create()
			return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		}

		private static string GetHashString(string inputString)
		{
			var sb = new StringBuilder();
			foreach (byte b in GetHash(inputString))
				sb.Append(b.ToString("X2"));

			return sb.ToString();
		}
	}
}