using Modules.Hub.Domain.Users.DomainEvents;

namespace Modules.Hub.Domain.Users;

public sealed class User : AggregateRoot<UserId>, IAuditableEntity
{
	private User(UserId id)
		: base(id)
	{
	}

	public string EmailAddress { get; private init; } = default!;

	public string FirstName { get; private init; } = default!;

	public string LastName { get; private init; } = default!;

	public string Password { get; private init; } = default!;

	public DateTimeOffset CreatedAt { get; private set; }

	public DateTimeOffset? UpdatedAt { get; private set; }

	public static User Create(string emailAddress
		, string firstName
		, string lastName
		, string password)
	{
		var user = new User(new UserId(Guid.NewGuid()))
		{
			EmailAddress = emailAddress,
			FirstName = firstName,
			LastName = lastName,
			Password = password,
		};

		user.RaiseDomainEvent(new UserSignedUpDomainEvent(Guid.NewGuid()
			, SystemTimeProvider.UtcNow
			, user.Id
			, user.EmailAddress
			, user.FirstName
			, user.LastName));

		return user;
	}
}
