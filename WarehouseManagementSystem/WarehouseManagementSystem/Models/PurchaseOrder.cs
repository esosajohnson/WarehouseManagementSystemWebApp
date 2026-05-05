using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class PurchaseOrder
{
    public int PurchaseOrderId { get; set; }

    public int SupplierId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ExpectedDate { get; set; }

    public PurchaseOrderStatus OrderStatus { get; set; } = PurchaseOrderStatus.Draft;

    public decimal TotalAmount { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual Supplier? Supplier { get; set; }
}
