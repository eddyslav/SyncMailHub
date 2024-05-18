using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Requests;

using Modules.Hub.Application.ServiceAccounts.AddServiceAccount.Google;

using Modules.Hub.Infrastucture.Google;

namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;

// replace with own implementation
internal sealed class GetGoogleServiceAccountAuthUrlCommandHandler(IHttpContextAccessor httpContextAccessor
	, IUserContextAccessor userContextAccessor
	, IOptions<GoogleOAuthConfiguration> options)
	: ICommandHandler<GetGoogleServiceAccountAuthUrlCommand, Uri>
{
	private readonly GoogleOAuthConfiguration configuration = options.GetConfiguration();

	private Result<Uri> Handle(string state) =>
		httpContextAccessor.GetSession()
			.SetGuidValue(SessionKeys.UserId, userContextAccessor.UserId.Value)
			.SetStringValue(SessionKeys.State, state)
			.Map(() => new GoogleAuthorizationCodeRequestUrl(new Uri(GoogleAuthConsts.AuthorizationUrl))
			{
				Scope = string.Join(" ", configuration.Scopes),
				AccessType = "offline",
				IncludeGrantedScopes = "true",
				ResponseType = "code",
				ClientId = configuration.ClientId,
				RedirectUri = configuration.RedirectUri.ToString(),
				Prompt = "consent",
				State = state,
			}.Build());

	public Task<Result<Uri>> Handle(GetGoogleServiceAccountAuthUrlCommand _, CancellationToken _1) =>
		Task.FromResult(Handle(Guid.NewGuid().ToString()));
}
