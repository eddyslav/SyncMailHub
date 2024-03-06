namespace Shared.Results.Errors;

public sealed record UnauthorizedError(string Code, string Message) : Error(Code, Message);
