namespace Modules.Hub.Domain.ServiceAccounts;

public sealed class GoogleServiceAccount : ServiceAccount
{
	private GoogleServiceAccount(ServiceAccountId id)
		: base(id)
	{
	}

	public string RefreshToken { get; private init; } = default!;

	public static GoogleServiceAccount Create(User user
		, string emailAddress
		, string externalId
		, string refreshToken)
	{
		var account = new GoogleServiceAccount(new ServiceAccountId(Guid.NewGuid()))
		{
			UserId = user.Id,
			EmailAddress = emailAddress,
			ExternalId = externalId,
			RefreshToken = refreshToken,
		};

		account.RaiseDomainEvent(new ServiceAccountAddedDomainEvent(Guid.NewGuid()
			, SystemTimeProvider.UtcNow
			, account.Id
			, account.EmailAddress
			, user.Id
			, user.EmailAddress
			, user.FirstName
			, user.LastName));

		return account;
	}
}
