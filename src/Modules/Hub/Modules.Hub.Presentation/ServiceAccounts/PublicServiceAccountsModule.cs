namespace Modules.Hub.Presentation.ServiceAccounts;

public sealed class PublicServiceAccountsModule : ICarterModule
{
	// this method can be rewritten if ms graph will be supported in this way as well
	private static Task<IResult> HandleCreateGoogleServiceAccountAsync(string code
		, string state
		, ISender sender
		, CancellationToken cancellationToken) =>
		Result.Create(new AddGoogleServiceAccountCommand(code, state))
			.Bind(command => sender.Send(command, cancellationToken))
			.Match(Results.Ok);

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/service-accounts");

		// can actually be placed as a single endpoint
		// e.g. /api/service-accounts/{type}/callback?...google/ms graph/etc specific query params
		// as well, as it can be a single entity as oauth2.0 will be used for the both providers
		group.MapGet("google/callback", HandleCreateGoogleServiceAccountAsync);
	}
}
