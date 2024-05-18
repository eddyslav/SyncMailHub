namespace Modules.Sync.Infrastructure.Emails.Google;

internal sealed class ServiceAccountSyncSessionConfigurationValidator : AbstractValidator<ServiceAccountSyncSessionConfiguration>
{
	public ServiceAccountSyncSessionConfigurationValidator()
	{
		RuleFor(x => x.MaxHistoriesToRetrievePerRequest)
			.GreaterThan(0);
	}
}
