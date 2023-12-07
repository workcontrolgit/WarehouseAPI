using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Wrappers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Features.Customers.Commands.DeleteCustomerById
{
    public class DeleteCustomerByIdCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; set; }

        public class DeleteCustomerByIdCommandHandler : IRequestHandler<DeleteCustomerByIdCommand, Response<Guid>>
        {
            private readonly ICustomerRepositoryAsync _repository;

            public DeleteCustomerByIdCommandHandler(ICustomerRepositoryAsync repository)
            {
                _repository = repository;
            }

            public async Task<Response<Guid>> Handle(DeleteCustomerByIdCommand command, CancellationToken cancellationToken)
            {
                var customer = await _repository.GetByIdAsync(command.Id);
                if (customer == null) throw new ApiException($"Customer Not Found.");
                await _repository.DeleteAsync(customer);
                return new Response<Guid>(customer.Id);
            }
        }
    }
}