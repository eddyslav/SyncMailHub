namespace Modules.Hub.Domain.Users;

public interface IUserRepository
{
	Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);

	Task<User?> GetByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default);

	Task<bool> CheckIfExistsByEmailAddressAsync(string emailAddress, CancellationToken cancellationToken = default);

	void Add(User user);
}
