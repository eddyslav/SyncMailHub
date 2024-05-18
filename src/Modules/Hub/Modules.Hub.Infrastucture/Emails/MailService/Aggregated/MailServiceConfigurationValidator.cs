namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class MailServiceConfigurationValidator : AbstractValidator<MailServiceConfiguration>
{
	public MailServiceConfigurationValidator()
	{
		RuleFor(x => x.EmailsCountCacheLifetime).GreaterThan(TimeSpan.Zero);

		RuleFor(x => x.EmailsFoldersCacheLifetime).GreaterThan(TimeSpan.Zero);

		RuleFor(x => x.EmailsFolderCountCacheLifetime).GreaterThan(TimeSpan.Zero);

		RuleFor(x => x.EmailsConversationsCacheLifetime).GreaterThan(TimeSpan.Zero);

		RuleFor(x => x.EmailsCacheLifeTime).GreaterThan(TimeSpan.Zero);
	}
}
