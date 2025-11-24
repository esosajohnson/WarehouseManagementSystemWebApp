using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class PickingTask
{
    public int PickingTaskId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public int? AssignedEmployeeId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public string? Notes { get; set; }

    public virtual Employee? AssignedEmployee { get; set; }

    public virtual Product Product { get; set; } = null!;
}
