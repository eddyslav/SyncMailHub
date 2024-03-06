namespace Modules.Hub.Infrastucture.PasswordHasherService;

internal sealed class BCryptPasswordHasherServiceConfiguration
{
	public enum PasswordHashType
	{
		SHA256 = 1,
		SHA384,
		SHA512
	}

	public required int WorkFactor { get; init; }

	public required PasswordHashType HashType { get; init; }
}
