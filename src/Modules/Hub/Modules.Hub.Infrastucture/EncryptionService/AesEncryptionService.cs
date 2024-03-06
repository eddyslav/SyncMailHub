using System.Text;

using System.Security.Cryptography;

namespace Modules.Hub.Infrastucture.EncryptionService;

internal sealed class AesEncryptionService(IOptions<AesEncryptionServiceConfiguration> options) : IEncryptionService
{
	private readonly byte[] key = Encoding.UTF8.GetBytes(options.GetConfiguration().EncryptionKey);

	public string Encrypt(string toEncrypt)
	{
		using var aes = Aes.Create();

		aes.Key = key;

		aes.GenerateIV();

		var encryptor = aes.CreateEncryptor();

		using var ms = new MemoryStream();

		var iv = aes.IV;
		ms.Write(iv, 0, iv.Length);

		using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
		using (var sw = new StreamWriter(cs))
		{
			sw.Write(toEncrypt);
		}

		var encrypted = ms.ToArray();
		return Convert.ToBase64String(encrypted);
	}

	public string Decrypt(string toDecryptBase64)
	{
		var toDecrypt = Convert.FromBase64String(toDecryptBase64);

		using var aes = Aes.Create();

		aes.Key = key;

		var iv = new byte[16];
		Array.Copy(toDecrypt, 0, iv, 0, iv.Length);

		aes.IV = iv;

		var actualToDecrypt = new byte[toDecrypt.Length - iv.Length];
		Array.Copy(toDecrypt, iv.Length, actualToDecrypt, 0, actualToDecrypt.Length);

		var decryptor = aes.CreateDecryptor();

		using var ms = new MemoryStream(actualToDecrypt);
		using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
		using var sr = new StreamReader(cs);

		return sr.ReadToEnd();
	}
}
