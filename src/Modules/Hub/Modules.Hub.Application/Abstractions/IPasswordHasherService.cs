namespace Modules.Hub.Application.Abstractions;

public interface IPasswordHasherService
{
	string HashPassword(string password);

	bool VerifyPassword(string password, string hashedPassword);
}
