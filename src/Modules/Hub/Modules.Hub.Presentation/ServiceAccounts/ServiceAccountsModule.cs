using Modules.Hub.Application.ServiceAccounts.DeleteServiceAccount;
using Modules.Hub.Application.ServiceAccounts.GetAllServiceAccounts;

using Modules.Hub.Presentation.ServiceAccounts.Contracts;

namespace Modules.Hub.Presentation.ServiceAccounts;

public sealed class ServiceAccountsModule : ICarterModule
{
	private static Task<IResult> HandleGetGoogleServiceAccountAuthUrlAsync(ISender sender, CancellationToken cancellationToken) =>
		Result.Create(GetGoogleServiceAccountAuthUrlCommand.Instance)
			.Bind(command => sender.Send(command, cancellationToken))
			.Map(url => new GoogleAuthUrlResponse(url))
			.Match(Results.Ok);

	private static Task<IResult> HandleGetAllAccountsPerUserAsync(ISender sender, CancellationToken cancellationToken) =>
		Result.Create(GetAllServiceAccountsQuery.Instance)
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleDeleteServiceAccountAsync(ISender sender
		, Guid accountId
		, CancellationToken cancellationToken) =>
		Result.Create(new DeleteServiceAccountCommand(new ServiceAccountId(accountId)))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.NoContent);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts")
			.RequireAuthorization();

		group.MapGet("google/authUrl", HandleGetGoogleServiceAccountAuthUrlAsync);
		group.MapGet("", HandleGetAllAccountsPerUserAsync);
		group.MapDelete("{accountId:guid}", HandleDeleteServiceAccountAsync);
	}
}
