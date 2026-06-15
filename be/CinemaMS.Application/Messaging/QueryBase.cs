using MediatR;

namespace CinemaMS.Application.Messaging;

public abstract class QueryBase<TResponse> : IRequest<TResponse>
{
}
