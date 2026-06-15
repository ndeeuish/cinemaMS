using MediatR;

namespace CinemaMS.Application.Messaging;

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand>
    where TCommand : CommandBase
{
    public abstract Task Handle(TCommand request, CancellationToken cancellationToken);
}

public abstract class CommandHandlerBase<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : CommandBase<TResponse>
{
    public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
}
