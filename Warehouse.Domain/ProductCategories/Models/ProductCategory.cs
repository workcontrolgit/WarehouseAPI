using Warehouse.Domain.Abstractions.Entities;

namespace Warehouse.Domain.ProductCategories.Models;

public class ProductCategory : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
}