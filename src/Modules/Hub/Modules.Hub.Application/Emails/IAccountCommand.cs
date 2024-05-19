namespace Modules.Hub.Application.Emails;

internal interface IAccountCommand<T> : ICommand<T>
	where T : notnull
{
	ServiceAccountId AccountId { get; }
}

internal interface IAccountCommand : ICommand
{
	ServiceAccountId AccountId { get; }
}
