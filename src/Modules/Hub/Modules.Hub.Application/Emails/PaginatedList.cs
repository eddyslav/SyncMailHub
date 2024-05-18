namespace Modules.Hub.Application.Emails;

public sealed record PaginatedList<T>(IReadOnlyList<T> Results, string? NextPageToken)
	where T : class;
