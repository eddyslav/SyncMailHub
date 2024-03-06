namespace Modules.Hub.Application.Emails;

public interface IMailServiceFactory
{
	Task<Result<IMailService>> CreateForAccountAsync(ServiceAccountId serviceAccountId, CancellationToken cancellationToken);
}
