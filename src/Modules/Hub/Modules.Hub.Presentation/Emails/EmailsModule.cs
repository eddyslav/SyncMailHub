using Modules.Hub.Application.Emails.DeleteEmail;
using Modules.Hub.Application.Emails.GetEmails;
using Modules.Hub.Application.Emails.GetEmailsCount;
using Modules.Hub.Application.Emails.SendEmail;

using Modules.Hub.Presentation.Emails.Contracts;

namespace Modules.Hub.Presentation.Emails;

public sealed class EmailsModule : ICarterModule
{
	private static Task<IResult> HandleGetEmailsCount(ISender sender
		, Guid accountId
		, CancellationToken cancellationToken) =>
		Result.Create(new GetEmailsCountQuery(new ServiceAccountId(accountId)))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleGetEmailsAsync(ISender sender
		, Guid accountId
		, string conversationId
		, CancellationToken cancellationToken) =>
		Result.Create(new GetEmailsQuery(new ServiceAccountId(accountId), conversationId))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleSendEmailAsync(ISender sender
		, Guid accountId
		, SendEmailRequest request
		, CancellationToken cancellationToken) =>
		Result.Create(new SendEmailCommand(new ServiceAccountId(accountId)
				, request.Subject
				, request.Body
				, request.To
				, request.Cc
				, request.Bcc))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleDeleteEmailByIdAsync(ISender sender
		, Guid accountId
		, string emailId
		, bool force
		, CancellationToken cancellationToken) =>
		Result.Create(new DeleteEmailCommand(new ServiceAccountId(accountId), emailId, force))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.NoContent);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts/{accountId:guid}")
			.RequireAuthorization();

		group.MapGet("emails/count", HandleGetEmailsCount);
		group.MapGet("conversations/{conversationId}/emails", HandleGetEmailsAsync);
		group.MapPost("emails", HandleSendEmailAsync);
		group.MapDelete("emails/{emailId}", HandleDeleteEmailByIdAsync);
	}
}
