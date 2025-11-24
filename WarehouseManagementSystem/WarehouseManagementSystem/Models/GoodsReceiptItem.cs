using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class GoodsReceiptItem
{
    public int GoodsReceiptItemId { get; set; }

    public int GoodsReceiptId { get; set; }

    public int ProductId { get; set; }

    public int QuantityReceived { get; set; }

    public int LocationId { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public string? Notes { get; set; }

    public virtual GoodsReceipt GoodsReceipt { get; set; } = null!;

    public virtual Location Location { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
