namespace Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

public sealed class GetGoogleServiceAccountAuthUrlCommand : ICommand<Uri>
{
	private GetGoogleServiceAccountAuthUrlCommand()
	{
	}

	public static GetGoogleServiceAccountAuthUrlCommand Instance { get; } = new();
}
