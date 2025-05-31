using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class Supplier
{
    public int SupplierId { get; set; }

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public virtual Product? Product { get; set; }
}
