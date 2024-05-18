namespace Modules.Hub.Application.Emails;

internal interface IAccountQuery<T> : IQuery<T>
	where T : notnull
{
	ServiceAccountId AccountId { get; }
}
