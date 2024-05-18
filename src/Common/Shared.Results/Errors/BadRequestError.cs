namespace Shared.Results.Errors;

public sealed record BadRequestError(string Code, string Message) : Error(Code, Message);
