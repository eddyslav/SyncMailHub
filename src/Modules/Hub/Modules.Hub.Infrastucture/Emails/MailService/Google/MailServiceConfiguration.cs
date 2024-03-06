namespace Modules.Hub.Infrastucture.Emails.MailService.Google;

internal sealed class MailServiceConfiguration
{
	public required int MaxEntitiesPerBatch { get; init; }
	public required int MaxRetriesToLoadEntities { get; init; }
}
