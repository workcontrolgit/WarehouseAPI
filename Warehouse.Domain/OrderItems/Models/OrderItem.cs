using System;
using Warehouse.Domain.Abstractions.Entities;
using Warehouse.Domain.Products.Models;

namespace Warehouse.Domain.OrderItems.Models
{
    public class OrderItem : AuditableBaseEntity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }

    }
}
