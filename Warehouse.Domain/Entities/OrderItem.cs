using Warehouse.Domain.Common;
using System;

namespace Warehouse.Domain.Entities
{
    public class OrderItem : AuditableBaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

    }
}
