namespace Modules.Hub.Application.Abstractions;

public interface IEncryptionService
{
	string Encrypt(string toEncrypt);

	string Decrypt(string toDecrypt);
}
