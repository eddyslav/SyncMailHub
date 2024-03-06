using System.Runtime.CompilerServices;

namespace Modules.Sync.Application.Emails;

public interface IServiceAccountSyncSession : IDisposable
{
	IAsyncEnumerable<Result<IReadOnlyList<IServiceAccountStateChange>>> ProcessStateChangesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default);
}
