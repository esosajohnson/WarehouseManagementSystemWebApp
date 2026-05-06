using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class ShipmentItem
{
    public int ShipmentItemId { get; set; }

    public int ShipmentId { get; set; }

    public int ProductId { get; set; }

    public int LocationId { get; set; }

    public int QuantityShipped { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual Location Location { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    public virtual Shipment Shipment { get; set; } = null!;
}
