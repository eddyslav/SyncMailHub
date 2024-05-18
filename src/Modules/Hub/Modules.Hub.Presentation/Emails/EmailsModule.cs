using Modules.Hub.Application.Emails.GetEmails;
using Modules.Hub.Application.Emails.GetEmailsCount;

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

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts/{accountId:guid}")
			.RequireAuthorization();

		group.MapGet("emails/count", HandleGetEmailsCount);
		group.MapGet("conversations/{conversationId}/emails", HandleGetEmailsAsync);
	}
}
