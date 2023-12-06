using Warehouse.Domain.Common;

namespace Warehouse.Domain.Entities;

public class ProductCategory : AuditableBaseEntity
{
    public string Name { get; set; } = null!;
}