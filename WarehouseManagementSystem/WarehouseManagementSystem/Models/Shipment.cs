using System;
using System.Collections.Generic;

namespace WarehouseManagementSystem.Models;

public partial class Shipment
{
    public int ShipmentId { get; set; }

    public string? Carrier { get; set; }

    public string? TrackingNumber { get; set; }

    public DateTime ShippingDate { get; set; }

    public string? Destination { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();
}
