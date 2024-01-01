using System;
using System.Collections.Generic;
using Warehouse.Domain.Orders.Models;
using Warehouse.Domain.Abstractions.Entities;
using Warehouse.Domain.Addresses.Models;

namespace Warehouse.Domain.Customers.Models
{
    public class Customer : AuditableBaseEntity
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
