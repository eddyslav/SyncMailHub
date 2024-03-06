using BCrypt.Net;
using BCryptImpl = BCrypt.Net.BCrypt;

using static Modules.Hub.Infrastucture.PasswordHasherService.BCryptPasswordHasherServiceConfiguration;

namespace Modules.Hub.Infrastucture.PasswordHasherService;

internal sealed class BCryptPasswordHasherService : IPasswordHasherService
{
	private readonly int workFactor;
	private readonly HashType hashType;

	public BCryptPasswordHasherService(IOptions<BCryptPasswordHasherServiceConfiguration> options)
	{
		var configuration = options.GetConfiguration();

		workFactor = configuration.WorkFactor;

		var passwordHashType = configuration.HashType;
		hashType = passwordHashType switch
		{
			PasswordHashType.SHA256 => HashType.SHA256,
			PasswordHashType.SHA384 => HashType.SHA384,
			PasswordHashType.SHA512 => HashType.SHA512,
			_ => throw new ArgumentOutOfRangeException(nameof(options), passwordHashType, "Password hash type cannot be transformed to BCrypt.HashType"),
		};
	}

	public string HashPassword(string password) =>
		BCryptImpl.EnhancedHashPassword(password, workFactor, hashType);

	public bool VerifyPassword(string password, string hashedPassword) =>
		BCryptImpl.EnhancedVerify(password, hashedPassword, hashType);
}
