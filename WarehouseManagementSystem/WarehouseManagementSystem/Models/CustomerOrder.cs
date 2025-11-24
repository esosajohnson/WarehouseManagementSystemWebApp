using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class CustomerOrder
{
    public int CustomerOrderId { get; set; }

    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<CustomerOrderItem> CustomerOrderItems { get; set; } = new List<CustomerOrderItem>();
}
