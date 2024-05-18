using Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolders;
using Modules.Hub.Application.Emails.EmailsFolders.GetEmailsFolderCount;

namespace Modules.Hub.Presentation.EmailsFolders;

public sealed class EmailsFoldersModule : ICarterModule
{
	private static Task<IResult> HandleGetEmailsFoldersAsync(ISender sender
		, Guid accountId
		, CancellationToken cancellationToken) =>
		Result.Create(new GetEmailsFoldersQuery(new ServiceAccountId(accountId)))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleGetEmailsFolderCountAsync(ISender sender
		, Guid accountId
		, string folderId
		, CancellationToken cancellationToken) =>
		Result.Create(new GetEmailsFolderCountQuery(new ServiceAccountId(accountId), folderId))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts/{accountId:guid}/folders")
			.RequireAuthorization();

		group.MapGet("", HandleGetEmailsFoldersAsync);
		group.MapGet("{folderId}/statistics", HandleGetEmailsFolderCountAsync);
	}
}
