namespace Shared.Results.Errors;

public sealed record ExternalServiceCallError(string Code, string Message, string ServiceMessage) : Error(Code, Message)
{
	private static readonly string defaultMessage = "A request to the service has failed";

	public ExternalServiceCallError(string code, string serviceMessage) : this(code, defaultMessage, serviceMessage) { }
}
