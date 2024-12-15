using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Template.Utils
{
	public static class Crypto
	{
		public static string SHA1(string input)
		{
			return Encoding.UTF8.GetString(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(input)));
		}

		public static byte[] Encrypt(byte[] bytes)
		{
			var encrypted = new AESEncryptionAlgorithm().Encrypt(bytes);
			return encrypted;
		}

		public static string Decrypt(byte[] bytes)
		{
			var decrypted = new AESEncryptionAlgorithm().Decrypt(bytes);
			return Encoding.UTF8.GetString(decrypted);
		}
	}

	public class AESEncryptionAlgorithm
	{
		private const int ivSize = 16;
		private const int keySize = 16;
		private const int pwIterations = 100;

		protected static void CopyStream(Stream input, Stream output, int bufferSize)
		{
			byte[] buffer = new byte[bufferSize];
			int read;

			while ((read = input.Read(buffer, 0, bufferSize)) > 0)
			{
				output.Write(buffer, 0, read);
			}
		}

		public byte[] Encrypt(byte[] bytes)
		{
			using (var input = new MemoryStream(bytes))
			{
				using (var output = new MemoryStream())
				{
					Encrypt(input, output);
					return output.ToArray();
				}
			}
		}

		public byte[] Decrypt(byte[] bytes)
		{
			using (var input = new MemoryStream(bytes))
			{
				using (var output = new MemoryStream())
				{
					Decrypt(input, output);
					return output.ToArray();
				}
			}
		}

		public void Encrypt(Stream input, Stream output)
		{
			input.Position = 0;

			using (var alg = Aes.Create())
			{
				alg.Mode = CipherMode.CBC;
				alg.Padding = PaddingMode.PKCS7;
				alg.GenerateIV();

				var key = new Rfc2898DeriveBytes(new byte[] {
					0xFA, 0xC9, 0xDD, 0xBF,
					0x0B, 0x35, 0x77, 0xF3,
					0xCD, 0x46, 0xEB, 0x93,
					0xEA, 0xA4, 0x0F, 0xDA
				}, alg.IV, pwIterations);

				alg.Key = key.GetBytes(keySize);
				output.Write(alg.IV, 0, ivSize);

				using var encryptor = alg.CreateEncryptor();
				using var cs = new CryptoStream(output, encryptor, CryptoStreamMode.Write);
				CopyStream(input, cs, 2048);
			}
		}

		public void Decrypt(Stream input, Stream output)
		{
			using (var alg = Aes.Create())
			{
				var thisIV = new byte[ivSize];
				input.Read(thisIV, 0, ivSize);
				alg.IV = thisIV;

				var key = new Rfc2898DeriveBytes(new byte[] {
					0xFA, 0xC9, 0xDD, 0xBF,
					0x0B, 0x35, 0x77, 0xF3,
					0xCD, 0x46, 0xEB, 0x93,
					0xEA, 0xA4, 0x0F, 0xDA
				}, alg.IV, pwIterations);

				alg.Key = key.GetBytes(keySize);

				using var decryptor = alg.CreateDecryptor();
				using var cryptoStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
				CopyStream(cryptoStream, output, 2048);

			}
			output.Position = 0;
		}
	}
}