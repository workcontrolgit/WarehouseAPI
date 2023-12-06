using Warehouse.Domain.Common;
using Warehouse.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Warehouse.Domain.Entities
{
    public class Order : AuditableBaseEntity
    {
        public DateTime OrderDate { get; set; }
        public Terms Terms { get; set; }
        public Guid CustomerId { get; set; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
