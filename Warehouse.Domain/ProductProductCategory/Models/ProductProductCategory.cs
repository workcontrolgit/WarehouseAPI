using System;
using Warehouse.Domain.Abstractions.Entities;
using Warehouse.Domain.ProductCategories.Models;
using Warehouse.Domain.Products.Models;

namespace Warehouse.Domain.ProductProductCategory.Entities;

public class ProductProductCategory : AuditableBaseEntity
{
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }

    public Product Product { get; set; } = null!;
    public ProductCategory Category { get; set; } = null!;
}
