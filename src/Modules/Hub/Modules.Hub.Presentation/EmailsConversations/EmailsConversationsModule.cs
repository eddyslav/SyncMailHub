using Modules.Hub.Application.Emails.EmailsConversations.DeleteEmailsConversation;
using Modules.Hub.Application.Emails.EmailsConversations.GetEmailConversations;
using Modules.Hub.Application.Emails.SendReplyToConversation;

using Modules.Hub.Presentation.EmailsConversations.Contracts;

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

	private static Task<IResult> HandleReplyToConversationAsync(ISender sender
		, Guid accountId
		, string conversationId
		, SendReplyRequest request
		, CancellationToken cancellationToken) =>
		Result.Create(new SendReplyToConversationCommand(new ServiceAccountId(accountId)
				, conversationId
				, request.Body))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleDeleteByIdAsync(ISender sender
		, Guid accountId
		, string conversationId
		, bool force
		, CancellationToken cancellationToken) =>
		Result.Create(new DeleteEmailsConversationCommand(new ServiceAccountId(accountId), conversationId, force))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.NoContent);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts/{accountId:guid}")
			.RequireAuthorization();

		group.MapGet("folders/{folderId}/conversations", HandleGetEmailsConversationsAsync);
		group.MapPost("conversations/{conversationId}/reply", HandleReplyToConversationAsync);
		group.MapDelete("conversations/{conversationId}", HandleDeleteByIdAsync);
	}
}
