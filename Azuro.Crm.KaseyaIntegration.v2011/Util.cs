using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.KaseyaIntegration
{
	public static class Util
	{
		
		public static string HashToCoveredPassword(string UserName, ref string Password, string HashingAlgorithm)
		{
			string Rand = GenerateRandomNumber(8);

			Password = HashValues(Password, UserName, HashingAlgorithm);
			Password = HashValues(Password, Rand, HashingAlgorithm);

			return Rand;
		}

		private static string HashValues(string Value1, string Value2, string HashingAlgorithm)
		{
			string sHashingAlgorithm = "";
			if (String.IsNullOrEmpty(HashingAlgorithm))
				sHashingAlgorithm = "SHA-1";
			else
				sHashingAlgorithm = HashingAlgorithm;

			byte[] arrByte;

			if (sHashingAlgorithm == "SHA-1")
			{
				SHA1Managed hash = new SHA1Managed();
				arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value1 + Value2));
			}
			else
			{
				SHA256Managed hash = new SHA256Managed();
				arrByte = hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(Value1 + Value2));
			}

			string s = "";
			for (int i = 0; i < arrByte.Length; i++)
			{
				s += arrByte[i].ToString("x2");
			}
			return s;
		}

		private static string GenerateRandomNumber(int NumberOfDigits)
		{
			System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();

			byte[] numbers = new byte[NumberOfDigits * 2];
			rng.GetNonZeroBytes(numbers);

			string result = "";
			for (int i = 0; i < NumberOfDigits; i++)
			{
				result += numbers[i].ToString();
			}

			result = result.Replace("0", "");
			return result.Substring(1, NumberOfDigits);

		}
	}
}
