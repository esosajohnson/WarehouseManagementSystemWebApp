using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Aisle { get; set; }

    public string? Shelf { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
}
