namespace Application.Messaging;

public interface IQuery<T> : IRequest<Result<T>>
	where T : notnull;
