using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class InventoryTransaction
{
    public int TransactionId { get; set; }

    public int ProductId { get; set; }

    public int QuantityChanged { get; set; }

    public string? TransactionType { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? Notes { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual Product Product { get; set; } = null!;
}
