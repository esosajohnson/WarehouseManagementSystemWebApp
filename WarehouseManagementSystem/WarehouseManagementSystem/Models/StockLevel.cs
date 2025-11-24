using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class StockLevel
{
    public int StockLevelId { get; set; }

    public int ProductId { get; set; }

    public int LocationId { get; set; }

    public int QuantityChanged { get; set; }

    public virtual Location Location { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
