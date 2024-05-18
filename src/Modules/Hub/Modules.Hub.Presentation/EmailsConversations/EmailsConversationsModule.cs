using Modules.Hub.Application.Emails.EmailsConversations.GetEmailConversations;

namespace Modules.Hub.Presentation.EmailsConversations;

public sealed class EmailsConversationsModule : ICarterModule
{
	private static Task<IResult> HandleGetEmailsConversationsAsync(ISender sender
		, Guid accountId
		, string folderId
		, string? pageToken
		, CancellationToken cancellationToken) =>
		Result.Create(new GetEmailsConversationsQuery(new ServiceAccountId(accountId), folderId, pageToken))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts/{accountId:guid}/folders/{folderId}/conversations")
			.RequireAuthorization();

		group.MapGet("", HandleGetEmailsConversationsAsync);
	}
}
