using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Domain.Customers.Models;

namespace Warehouse.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Response<Customer>>
    {
        public Guid Id { get; set; }

        public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Response<Customer>>
        {
            private readonly ICustomerRepositoryAsync _positionRepository;

            public GetCustomerByIdQueryHandler(ICustomerRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Customer>> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(query.Id);
                if (position == null) throw new ApiException($"Customer Not Found.");
                return new Response<Customer>(position);
            }
        }
    }
}