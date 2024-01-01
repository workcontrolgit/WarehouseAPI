using MediatR;

namespace Warehouse.Application.Abstractions.Commands
{
    public abstract record Command : IRequest<Unit>;
}
