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

    public virtual Supplier ProductNavigation { get; set; } = null!;
}
