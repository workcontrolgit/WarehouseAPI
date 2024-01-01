﻿using Warehouse.Application.Abstractions.Repositories;
using Warehouse.Domain.Abstractions.DomainEvents;
using Warehouse.Domain.Abstractions.Entities;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Warehouse.Infrastructure.Persistence.Contexts;

namespace AspNetCore.EventSourcing.Infrastructure.Repositories
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IPublisher _mediator;

        public UnitOfWork(ApplicationDbContext context,
            IPublisher mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<bool> CommitAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // Integration Events will be stored in the IntegrationEventOutbox ready to be published later
            await DispatchEventsAsync();

            // After executing this line all the changes (from any Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        private async Task DispatchEventsAsync()
        {
            var processedDomainEvents = new List<DomainEvent>();
            var unprocessedDomainEvents = GetDomainEvents();
            // this is needed incase another DomainEvent is published from a DomainEventHandler
            while (unprocessedDomainEvents.Any())
            {
                await DispatchDomainEventsAsync(unprocessedDomainEvents);
                processedDomainEvents.AddRange(unprocessedDomainEvents);
                unprocessedDomainEvents = GetDomainEvents()
                                            .Where(e => !processedDomainEvents.Contains(e))
                                            .ToList();
            }

            ClearDomainEvents();
        }

        private List<DomainEvent> GetDomainEvents()
        {
            var aggregateRoots = GetTrackedAggregateRoots();
            return aggregateRoots
                .SelectMany(x => x.DomainEvents)
                .ToList();
        }

        private List<AggregateRoot> GetTrackedAggregateRoots()
        {
            return _context.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();
        }

        public async Task DispatchDomainEventsAsync(List<DomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }

        private void ClearDomainEvents()
        {
            var aggregateRoots = GetTrackedAggregateRoots();
            aggregateRoots.ForEach(aggregate => aggregate.ClearDomainEvents());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
