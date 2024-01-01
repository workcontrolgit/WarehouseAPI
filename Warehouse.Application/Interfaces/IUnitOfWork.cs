using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Warehouse.Domain.Abstractions.DomainEvents;

namespace Warehouse.Application.Abstractions.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync(CancellationToken cancellationToken = default);
        Task DispatchDomainEventsAsync(List<DomainEvent> domainEvents);
    }
}
