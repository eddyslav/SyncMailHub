namespace Modules.Hub.Communication.RegisterServiceAccountStateChange;

public sealed class RegisterMailboxStateChangeResponse
{
	private RegisterMailboxStateChangeResponse()
	{
	}

	public static RegisterMailboxStateChangeResponse Instance { get; } = new();
}
