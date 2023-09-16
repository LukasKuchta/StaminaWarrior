using MediatR;

namespace Backend.Application.Contracts;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
