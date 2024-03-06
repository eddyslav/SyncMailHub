namespace Modules.Sync.Infrastructure;

internal sealed class ServiceAccountContextAccessor : IServiceAccountContextAccessor
{
	private static readonly AsyncLocal<ServiceAccount> account = new();

	public ServiceAccount Account
	{
		get => account.Value ?? throw new InvalidOperationException("Context account was not set");
		set
		{
			if (account.Value is not null)
			{
				throw new InvalidOperationException("Context account was already set");
			}

			account.Value = value;
		}
	}
}
