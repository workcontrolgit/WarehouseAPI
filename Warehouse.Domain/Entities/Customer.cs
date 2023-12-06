using Warehouse.Domain.Common;
using System;
using System.Collections.Generic;

namespace Warehouse.Domain.Entities
{
    public class Customer : AuditableBaseEntity
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string ContactName { get; set; }
        public Guid AddressId { get; set; }
        public Address? Address { get; set; }
        public IList<Order> Orders { get; set; } = new List<Order>();
    }
}
