namespace Modules.Hub.Infrastucture.Emails.MailService.Aggregated;

internal sealed class MailServiceConfiguration
{
	public required TimeSpan EmailsCountCacheLifetime { get; init; }

	public required TimeSpan EmailsFoldersCacheLifetime { get; init; }

	public required TimeSpan EmailsFolderCountCacheLifetime { get; init; }

	public required TimeSpan EmailsConversationsCacheLifetime { get; init; }

	public required TimeSpan EmailsCacheLifeTime { get; init; }
}
