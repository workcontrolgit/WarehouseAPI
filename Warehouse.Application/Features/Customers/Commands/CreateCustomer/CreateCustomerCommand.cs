using AutoMapper;
using MediatR;
using Warehouse.Application.Interfaces.Repositories;
using Warehouse.Application.Wrappers;
using Warehouse.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Features.Customers.Commands.CreateCustomer
{

    public partial class CreateCustomerCommand : Customer, IRequest<Response<Customer>>
    {
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Response<Customer>>
    {
        private readonly ICustomerRepositoryAsync _repository;
        private readonly IMapper _mapper;

        public CreateCustomerCommandHandler(ICustomerRepositoryAsync repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<Customer>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);
            await _repository.AddAsync(customer);
            return new Response<Customer>(customer);
        }
    }


}