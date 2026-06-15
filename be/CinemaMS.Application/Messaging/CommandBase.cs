using MediatR;

namespace CinemaMS.Application.Messaging;

public abstract class CommandBase : IRequest
{
}

public abstract class CommandBase<TResponse> : IRequest<TResponse>
{
}
