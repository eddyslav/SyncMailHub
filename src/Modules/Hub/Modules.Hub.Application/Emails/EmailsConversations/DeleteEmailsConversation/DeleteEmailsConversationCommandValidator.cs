namespace Modules.Hub.Application.Emails.EmailsConversations.DeleteEmailsConversation;

internal sealed class DeleteEmailsConversationCommandValidator : AbstractValidator<DeleteEmailsConversationCommand>
{
	public DeleteEmailsConversationCommandValidator()
	{
		RuleFor(x => x.AccountId).NotNull();
		RuleFor(x => x.ConversationId).NotEmpty();
	}
}
