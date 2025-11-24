using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class GoodsReceipt
{
    public int GoodsReceiptId { get; set; }

    public int? PurchaseOrderId { get; set; }

    public int SupplierId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime ReceiptDate { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? Notes { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<GoodsReceiptItem> GoodsReceiptItems { get; set; } = new List<GoodsReceiptItem>();

    public virtual PurchaseOrder? PurchaseOrder { get; set; }

    public virtual Supplier Supplier { get; set; } = null!;
}
