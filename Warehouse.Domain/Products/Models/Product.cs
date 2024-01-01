using System;
using System.Collections.Generic;
using Warehouse.Domain.ProductProductCategory.Entities;
using Warehouse.Domain.Abstractions.Entities;

namespace Warehouse.Domain.Products.Models;

public class Product : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
    public DateTimeOffset CreationDate { get; set; }
    public ICollection<ProductProductCategory.Entities.ProductProductCategory> ProductProductCategories { get; set; } = new List<ProductProductCategory.Entities.ProductProductCategory>();
    public string Description { get; set; } = null!;
}
