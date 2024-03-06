using Google;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;

using Modules.Hub.Infrastucture.Google;

namespace Modules.Hub.Infrastucture.ServiceAccounts.AddServiceAccount.Google;

internal sealed class GoogleServiceAccountCredentialsVerifier(ILogger logger, IOptions<GoogleOAuthConfiguration> options)
{
	private const string currentUserId = "me";

	private readonly ILogger logger = logger.ForContext<GoogleServiceAccountCredentialsVerifier>();
	private readonly GoogleOAuthConfiguration configuration = options.GetConfiguration();

	// probably replace it with http client
	private async Task<Result<TokenResponse>> ExchangeCodeForTokenAsync(string code, CancellationToken cancellationToken)
	{
		var initializer = new GoogleAuthorizationCodeFlow.Initializer
		{
			ClientSecrets = new ClientSecrets
			{
				ClientId = configuration.ClientId,
				ClientSecret = configuration.ClientSecret,
			},
		};

		var flow = new GoogleAuthorizationCodeFlow(initializer);

		try
		{
			return await flow.ExchangeCodeForTokenAsync(currentUserId
				, code
				, configuration.RedirectUri.ToString()
				, cancellationToken);
		}
		catch (TokenResponseException ex)
		{
			logger.Error(ex, "Failed to verify mailbox credentials");

			return Result.Failure<TokenResponse>(ServiceAccountErrors.InvalidCredentials);
		}
		catch (GoogleApiException ex)
		{
			logger.Error(ex, "Failed to verify mailbox credentials");

			return Result.Failure<TokenResponse>(ServiceAccountErrors.Google.ApiRequestFailed);
		}
	}

	private Result<GoogleMailboxCredentialsVerifyResult> ParseResult(TokenResponse tokenResponse)
	{
		var idToken = tokenResponse.IdToken;

		var tokenHandler = new JwtSecurityTokenHandler();
		if (!tokenHandler.CanReadToken(idToken))
		{
			logger.Error("Invalid id token retrieved");
			return Result.Failure<GoogleMailboxCredentialsVerifyResult>(ServiceAccountErrors.Google.TokenError);
		}

		var parsedToken = tokenHandler.ReadJwtToken(idToken);
		var subject = parsedToken.Subject;
		if (string.IsNullOrWhiteSpace(subject))
		{
			logger.Error("Invalid \"sub\" claim on token");
			return Result.Failure<GoogleMailboxCredentialsVerifyResult>(ServiceAccountErrors.Google.TokenError);
		}

		if (!parsedToken.Payload.TryGetValue("email", out var rawEmailAddress)
			|| rawEmailAddress is not string emailAddress)
		{
			logger.Error("Id token payload does not contain email address information");
			return Result.Failure<GoogleMailboxCredentialsVerifyResult>(ServiceAccountErrors.Google.TokenError);
		}

		return new GoogleMailboxCredentialsVerifyResult(emailAddress, subject, tokenResponse.RefreshToken);
	}

	public Task<Result<GoogleMailboxCredentialsVerifyResult>> VerifyCredentialsAsync(string code, CancellationToken cancellationToken) =>
		Result.Create(code)
			.Bind(code => ExchangeCodeForTokenAsync(code, cancellationToken))
			.Bind(ParseResult);
}
