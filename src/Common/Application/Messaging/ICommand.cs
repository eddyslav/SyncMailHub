namespace Application.Messaging;

public interface ICommand<T> : IRequest<Result<T>>
	where T : notnull;

public interface ICommand : IRequest<Result>;
