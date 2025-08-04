using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string Sku { get; set; } = null!;

    public int Quantity { get; set; }

    [Required(ErrorMessage = "The Supplier field is required.")]
    public int? SupplierId { get; set; }

    [Required(ErrorMessage = "The Location field is required.")]
    public int? LocationId { get; set; }

    [Required(ErrorMessage = "The Category field is required.")]
    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual Location? Location { get; set; }

    public virtual Supplier? Supplier { get; set; }
}
