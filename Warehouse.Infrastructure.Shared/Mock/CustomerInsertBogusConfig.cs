using Bogus;
using Warehouse.Domain.Entities;
using System;


namespace Warehouse.Infrastructure.Shared.Mock
{
    public class CustomerInsertBogusConfig : Faker<Customer>
    {
        public CustomerInsertBogusConfig()
        {
            RuleFor(o => o.Id, f => Guid.NewGuid());
            RuleFor(o => o.CompanyName, f => f.Name.JobTitle());
            RuleFor(o => o.ContactName, f => f.Name.FullName());
            RuleFor(o => o.Phone, f => f.Name.LastName());
            RuleFor(o => o.Created, f => f.Date.Past(1));
            RuleFor(o => o.CreatedBy, f => f.Name.FullName());
            RuleFor(o => o.LastModified, f => f.Date.Recent(1));
            RuleFor(o => o.LastModifiedBy, f => f.Name.FullName());
        }
    }
}
