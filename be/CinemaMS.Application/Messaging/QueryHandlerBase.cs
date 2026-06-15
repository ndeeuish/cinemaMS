using MediatR;

namespace CinemaMS.Application.Messaging;

public abstract class QueryHandlerBase<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : QueryBase<TResponse>
{
    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
}
