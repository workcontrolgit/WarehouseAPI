using Warehouse.Domain.Abstractions.Entities;

namespace Warehouse.Domain.Addresses.Models
{
    public class Address : AuditableBaseEntity
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }
}
