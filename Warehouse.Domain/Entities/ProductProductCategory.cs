using Warehouse.Domain.Common;
using System;

namespace Warehouse.Domain.Entities;

public class ProductProductCategory : AuditableBaseEntity
{
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }

    public Product Product { get; set; } = null!;
    public ProductCategory Category { get; set; } = null!;
}
