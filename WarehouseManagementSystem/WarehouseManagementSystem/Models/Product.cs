using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    public int? SupplierId { get; set; }

    public int? LocationId { get; set; }

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual Location? Location { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
