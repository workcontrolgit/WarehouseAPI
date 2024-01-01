using MediatR;
using System;

namespace Warehouse.Domain.Abstractions.DomainEvents
{
    public abstract record DomainEvent() : INotification
    {
        public Guid EventId { get; } = Guid.NewGuid();
    }
}
