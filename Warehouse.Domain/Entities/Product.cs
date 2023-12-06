using Warehouse.Domain.Common;
using System;
using System.Collections.Generic;

namespace Warehouse.Domain.Entities;

public class Product : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
    public DateTimeOffset CreationDate { get; set; }
    public ICollection<ProductProductCategory> ProductProductCategories { get; set; } = new List<ProductProductCategory>();
    public string Description { get; set; } = null!;
}
