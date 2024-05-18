namespace Shared.Results.Errors;

public sealed record NotFoundError(string Code, string Message) : Error(Code, Message);
