using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Position { get; set; }

    public DateTime? HireDate { get; set; }

    public bool IsActive { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public virtual ICollection<GoodsReceipt> GoodsReceipts { get; set; } = new List<GoodsReceipt>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<PickingTask> PickingTasks { get; set; } = new List<PickingTask>();

    public virtual ICollection<ReturnTransaction> ReturnTransactions { get; set; } = new List<ReturnTransaction>();
}
