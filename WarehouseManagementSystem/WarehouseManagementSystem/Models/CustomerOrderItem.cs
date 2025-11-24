using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class CustomerOrderItem
{
    public int CustomerOrderItemId { get; set; }

    public int CustomerOrderId { get; set; }

    public int ProductId { get; set; }

    public int QuantityChanged { get; set; }

    public int QuantityShipped { get; set; }

    public virtual CustomerOrder CustomerOrder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
