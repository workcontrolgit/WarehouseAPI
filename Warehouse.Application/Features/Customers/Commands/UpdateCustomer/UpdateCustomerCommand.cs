using MediatR;
using Warehouse.Application.Exceptions;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Wrappers;
using Warehouse.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : Customer, IRequest<Response<Guid>>
    {
        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Response<Guid>>
        {
            private readonly ICustomerRepositoryAsync _repository;

            public UpdateCustomerCommandHandler(ICustomerRepositoryAsync repository)
            {
                _repository = repository;
            }

            public async Task<Response<Guid>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                var customer = await _repository.GetByIdAsync(command.Id);

                if (customer == null)
                {
                    throw new ApiException($"Customer Not Found.");
                }
                else
                {
                    customer.CompanyName = command.CompanyName;
                    customer.Phone = command.Phone;
                    customer.ContactName = command.ContactName;
                    await _repository.UpdateAsync(customer);
                    return new Response<Guid>(customer.Id);
                }
            }
        }
    }
}