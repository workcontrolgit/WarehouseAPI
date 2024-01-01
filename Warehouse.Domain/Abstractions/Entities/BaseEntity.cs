using System;

namespace Warehouse.Domain.Abstractions.Entities
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}