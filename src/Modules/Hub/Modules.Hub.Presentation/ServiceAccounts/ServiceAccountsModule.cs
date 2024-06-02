using Modules.Hub.Presentation.ServiceAccounts.Contracts;

using Modules.Hub.Application.ServiceAccounts.GetAllServiceAccounts;
using Modules.Hub.Application.ServiceAccounts.GetServiceAccountById;
using Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;
using Modules.Hub.Application.ServiceAccounts.DeleteServiceAccount;

namespace Modules.Hub.Presentation.ServiceAccounts;

public sealed class ServiceAccountsModule : ICarterModule
{
	private static Task<IResult> HandleGetAllAccountsPerUserAsync(ISender sender, CancellationToken cancellationToken) =>
		Result.Create(GetAllServiceAccountsQuery.Instance)
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleGetServiceAccountByIdAsync(ISender sender
		, Guid accountId
		, CancellationToken cancellationToken) =>
		Result.Create(new GetServiceAccountByIdQuery(new ServiceAccountId(accountId)))
			.Bind(query => sender.Send(query, cancellationToken))
			.Match(Results.Ok);

	private static Task<IResult> HandleCreateGoogleServiceAccountAsync(AddGoogleServiceAccountRequest request
		, ISender sender
		, CancellationToken cancellationToken) =>
		Result.Create(new AddGoogleServiceAccountCommand(request.Code))
			.Bind(command => sender.Send(command, cancellationToken))
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

		group.MapGet("", HandleGetAllAccountsPerUserAsync);
		group.MapGet("{accountId:guid}", HandleGetServiceAccountByIdAsync);
		group.MapPost("google", HandleCreateGoogleServiceAccountAsync);
		group.MapDelete("{accountId:guid}", HandleDeleteServiceAccountAsync);
	}
}
