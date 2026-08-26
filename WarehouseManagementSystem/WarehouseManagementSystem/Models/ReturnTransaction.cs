using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class ReturnTransaction
{
    public int ReturnTransactionId { get; set; }

    public int ProductId { get; set; }

    public int? LocationId { get; set; }

    public int? ShipmentId { get; set; }

    public int Quantity { get; set; }

    public string? ReturnReason { get; set; }

    public DateTime ReturnDate { get; set; }

    public int? ProcessedByEmployeeId { get; set; }

    public ReturnStatus ConditionStatus { get; set; } = ReturnStatus.Pending;

    public string? Notes { get; set; }

    public virtual Employee? ProcessedByEmployee { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Shipment? Shipment { get; set; }
}
