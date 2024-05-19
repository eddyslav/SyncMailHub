namespace Modules.Hub.Application.Emails.SendReplyToConversation;

internal sealed class SendReplyToConversationCommandValidator : AbstractValidator<SendReplyToConversationCommand>
{
	public SendReplyToConversationCommandValidator()
	{
		RuleFor(x => x.AccountId).NotNull();
		RuleFor(x => x.ConversationId).NotEmpty();
		RuleFor(x => x.Body).NotEmpty();
	}
}
