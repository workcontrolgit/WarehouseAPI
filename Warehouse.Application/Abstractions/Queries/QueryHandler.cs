using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Abstractions.Queries
{
    public abstract class QueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, TResponse> where TQuery : Query<TResponse>
    {
        protected readonly IMapper? Mapper;

        public QueryHandler(IMapper? mapper)
        {
            Mapper = mapper;
        }

        public async Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken)
        {
            return await HandleAsync(request);
        }

        protected abstract Task<TResponse> HandleAsync(TQuery request);
    }
}
