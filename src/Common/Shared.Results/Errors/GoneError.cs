namespace Shared.Results.Errors;

public sealed record GoneError(string Code, string Message) : Error(Code, Message);
