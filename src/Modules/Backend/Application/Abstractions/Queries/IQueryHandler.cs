using Backend.Application.Contracts;
using MediatR;

namespace Backend.Application.Abstractions.Queries;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
where TQuery : IQuery<TResponse>
{
}
