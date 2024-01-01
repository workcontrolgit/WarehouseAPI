using Warehouse.Domain.Enums;
using System;
using System.Collections.Generic;
using Warehouse.Domain.Abstractions.Entities;
using Warehouse.Domain.OrderItems.Models;

namespace Warehouse.Domain.Orders.Models
{
    public class Order : AuditableBaseEntity
    {
        public DateTime OrderDate { get; set; }
        public Terms Terms { get; set; }
        public Guid CustomerId { get; set; }
        public IList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
