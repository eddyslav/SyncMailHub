namespace Modules.Hub.Persistence.Repositories;

public sealed class UserRepository(HubDbContext dbContext) : IUserRepository
{
	public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default) =>
		dbContext.Users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);

	public Task<User?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default) =>
		dbContext.Users.FirstOrDefaultAsync(user => user.EmailAddress == emailAddress, cancellationToken);

	public Task<bool> CheckIfExistsByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default) =>
		dbContext.Users.AnyAsync(user => user.EmailAddress == emailAddress, cancellationToken);

	public void Add(User user) => dbContext.Users.Add(user);
}
