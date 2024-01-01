using MediatR;

namespace Warehouse.Application.Abstractions.Queries
{
    public abstract record Query<T> : IRequest<T>;
}
