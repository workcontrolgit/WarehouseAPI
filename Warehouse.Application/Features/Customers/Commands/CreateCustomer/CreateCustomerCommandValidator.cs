using FluentValidation;
using Warehouse.Application.Interfaces.Repositories;
using System.Threading;
using System.Threading.Tasks;

namespace Warehouse.Application.Features.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        private readonly ICustomerRepositoryAsync repository;

        public CreateCustomerCommandValidator(ICustomerRepositoryAsync repository)
        {
            this.repository = repository;

            RuleFor(p => p.CompanyName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueCustomerNumber).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.ContactName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }

        private async Task<bool> IsUniqueCustomerNumber(string customerNumber, CancellationToken cancellationToken)
        {
            return await repository.IsUniqueCustomerNumberAsync(customerNumber);
        }
    }
}