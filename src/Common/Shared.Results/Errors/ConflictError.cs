namespace Shared.Results.Errors;

public sealed record ConflictError(string Code, string Message) : Error(Code, Message);
