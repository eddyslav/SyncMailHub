namespace Modules.Hub.Infrastucture.Emails.MailService;

internal sealed class ServiceAccountContextAccessor : IServiceAccountContextAccessor
{
	private ServiceAccountId? accountId;

	public ServiceAccountId AccountId
	{
		get => accountId ?? throw new InvalidOperationException("Context account was not set");
		set
		{
			if (accountId is not null)
			{
				throw new InvalidOperationException("Context account was already set");
			}

			accountId = value;
		}
	}
}
